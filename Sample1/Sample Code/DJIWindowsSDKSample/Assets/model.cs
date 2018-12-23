using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.AI.MachineLearning;
namespace DJIWindowsSDKSample
{
    
    public sealed class modelInput
    {
        public TensorFloat Input3; // shape(1,1,28,28)
    }
    
    public sealed class modelOutput
    {
        public TensorFloat Plus214_Output_0; // shape(1,10)
    }
    
    public sealed class modelModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static async Task<modelModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            modelModel learningModel = new modelModel();
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
