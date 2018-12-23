using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.AI.MachineLearning;
namespace DJIWindowsSDKSample
{
    
    public sealed class resnet18v2Input
    {
        public TensorFloat data; // shape(1,3,224,224)
    }
    
    public sealed class resnet18v2Output
    {
        public TensorFloat resnetv22_dense0_fwd; // shape(1,1000)
    }
    
    public sealed class resnet18v2Model
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static async Task<resnet18v2Model> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            resnet18v2Model learningModel = new resnet18v2Model();
            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }
        public async Task<resnet18v2Output> EvaluateAsync(resnet18v2Input input)
        {
            binding.Bind("data", input.data);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new resnet18v2Output();
            output.resnetv22_dense0_fwd = result.Outputs["resnetv22_dense0_fwd"] as TensorFloat;
            return output;
        }
    }
}
