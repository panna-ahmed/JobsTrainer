using Microsoft.ML;
using System;
using System.IO;
using Microsoft.ML.Data;
public partial class MLModel
{
    /// <summary>
    /// model input class for MLModel.
    /// </summary>
    #region model input class
    public class ModelInput
    {
        [ColumnName(@"Sample")]
        public string Sample { get; set; }
    }

    #endregion

    /// <summary>
    /// model output class for MLModel.
    /// </summary>
    #region model output class
    public class ModelOutput
    {        
        [ColumnName(@"PredictedLabel")]
        public bool PredictedLabel { get; set; }

        [ColumnName(@"Score")]
        public float Score { get; set; }

        [ColumnName(@"Probability")]
        public float Probability { get; set; }

    }

    #endregion

    private static string MLNetModelPath = Path.GetFullPath("MLModel.zip");

    public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

    /// <summary>
    /// Use this method to predict on <see cref="ModelInput"/>.
    /// </summary>
    /// <param name="input">model input.</param>
    /// <returns><seealso cref=" ModelOutput"/></returns>
    public static ModelOutput Predict(ModelInput input)
    {
        var predEngine = PredictEngine.Value;
        return predEngine.Predict(input);
    }

    private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
    {
        var mlContext = new MLContext();
        ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
        return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
    }
}
