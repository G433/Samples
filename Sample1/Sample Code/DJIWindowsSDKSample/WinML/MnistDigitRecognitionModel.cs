using System;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Media;
using Windows.Storage.Streams;

namespace DJIWindowsSDKSample.WinML
{
    public sealed class modelInput
    {
        public VideoFrame Input3; // shape(1,1,28,28)
    }
    
    public sealed class modelOutput
    {
        public TensorFloat Plus214_Output_0; // shape(1,10)
    }
    
    public sealed class MnistDigitRecognitionModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static async Task<MnistDigitRecognitionModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            MnistDigitRecognitionModel learningModel = new MnistDigitRecognitionModel();
            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }
        public async Task<modelOutput> EvaluateAsync(modelInput input)
        {
            binding.Bind("Input3", input.Input3);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new modelOutput();
            output.Plus214_Output_0 = result.Outputs["Plus214_Output_0"] as TensorFloat;
            return output;
        }
    }
}
