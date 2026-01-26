using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Steps;
using OcrFlow.Core.Flow.Steps.Execution;

namespace OcrFlow.Core.Flow
{
    public sealed class OcrProcessFlow : IOcrProcessFlow
    {
        private readonly IOcrStepFactory _factory;
        private readonly List<Func<IOcrStepFactory, IOcrStep>> _stepFactories = [];

        private OcrProcessFlow(IOcrStepFactory factory)
            => _factory = factory;

        public static OcrProcessFlow Start(IOcrStepFactory factory)
            => new(factory);

        public IOcrProcessFlow LoadImage()
            => Add<LoadImageStep>();

        public IOcrProcessFlow Rotate()
            => Add<RotateStep>();

        public IOcrProcessFlow Deskew()
            => Add<DeskewStep>();

        public IOcrProcessFlow NormalizeDpi()
            => Add<NormalizeDpiStep>();

        public IOcrProcessFlow TesseractOcr()
            => Add<TesseractOcrStep>();

        public OcrProcess Build()
        {
            var steps = new IOcrStep[_stepFactories.Count];

            for (int i = 0; i < _stepFactories.Count; i++)
                steps[i] = _stepFactories[i](_factory);

            return new OcrProcess(steps);
        }

        private IOcrProcessFlow Add<T>() where T : IOcrStep
        {
            _stepFactories.Add(f => f.Create<T>());
            return this;
        }
    }
}
