using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ClassSwitcherOptions _option;

        public ClassSwitcher(IEnumerable<TInterface> classes, IFeatureManagerSnapshot featureManager, IEnumerable<IOptions<ClassSwitcherOptions>> options)
        {
            var classList = classes?.ToList() ?? throw new ArgumentNullException(nameof(classes));

            if (classList.Count < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(classes), $"Minimum number of expected classes of {typeof(TInterface)} is 2, found {classList.Count}");
            }

            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));

            options = options?.ToList() ?? throw new ArgumentNullException(nameof(options));

            if (!options.Any())
            {
                throw new ArgumentNullException(nameof(options));
            }

            _option = options.FirstOrDefault(item => item.Value.Interface == typeof(TInterface))?.Value ?? throw new ArgumentOutOfRangeException(nameof(options), $"No options found for {typeof(TInterface)}");

            _flagDisabledClass = classList.FirstOrDefault(item => item.GetType() == typeof(TFlagDisabledClass));

            _flagEnabledClass = classList.FirstOrDefault(item => item.GetType() == typeof(TFlagEnabledClass));
        }

        public async Task<TInterface> GetActiveClass() =>
        await _featureManager.IsEnabledAsync(_option.Flag)
            ? _flagEnabledClass
            : _flagDisabledClass;

    }
}
