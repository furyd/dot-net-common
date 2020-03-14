using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace ReservoirDevs.FeatureManagement
{
    public class ClassSwitcher<TInterface, TFlagEnabledClass, TFlagDisabledClass>
        where TInterface : class
        where TFlagEnabledClass : TInterface
        where TFlagDisabledClass : TInterface
    {
        private readonly TInterface _flagDisabledClass;
        private readonly TInterface _flagEnabledClass;
        private readonly IFeatureManagerSnapshot _featureManager;
        private readonly ClassSwitcherOptions<TInterface> _options;
        private readonly ILogger<ClassSwitcher<TInterface, TFlagEnabledClass, TFlagDisabledClass>> _logger;

        public ClassSwitcher(IEnumerable<TInterface> classes, IFeatureManagerSnapshot featureManager, IOptions<ClassSwitcherOptions<TInterface>> options, ILogger<ClassSwitcher<TInterface, TFlagEnabledClass, TFlagDisabledClass>> logger)
        {
            var classList = classes?.ToList() ?? throw new ArgumentNullException(nameof(classes));

            if (classList.Count < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(classes), $"Minimum number of expected classes of {typeof(TInterface)} is 2, found {classList.Count}");
            }

            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));

            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _flagDisabledClass = classList.FirstOrDefault(item => item.GetType() == typeof(TFlagDisabledClass));

            _flagEnabledClass = classList.FirstOrDefault(item => item.GetType() == typeof(TFlagEnabledClass));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TInterface> GetActiveClass() =>
        await _featureManager.IsEnabledAsync(_options.Flag)
            ? _flagEnabledClass
            : _flagDisabledClass;

    }
}
