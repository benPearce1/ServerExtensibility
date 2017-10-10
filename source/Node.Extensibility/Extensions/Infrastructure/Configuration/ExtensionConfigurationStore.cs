using System;
using System.Collections.Generic;
using Nevermore.Contracts;
using Octopus.Data.Storage.Configuration;

namespace Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration
{
    public abstract class ExtensionConfigurationStore<TConfiguration, TResource> : IExtensionConfigurationStore, IHasConfigurationSettings
        where TConfiguration : ExtensionConfigurationDocument
        where TResource : IId
    {
        readonly IConfigurationStore configurationStore;

        protected ExtensionConfigurationStore(IConfigurationStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        public object GetConfiguration()
        {
            var doc = configurationStore.Get<TConfiguration>(Id);
            return MapToResource(doc);
        }

        protected abstract TResource MapToResource(TConfiguration doc);

        public void SetConfiguration(object config)
        {
            if (config is TResource == false)
            {
                throw new ArgumentException($"Given config type is {config.GetType()}, but {typeof(TResource)} was expected");
            }

            var resource = (TResource) config;
            if (resource.Id != Id)
            {
                throw new ArgumentException($"The given Id of {resource.Id} is not valid, {Id} was expected");
            }
            var newDoc = MapFromResource(resource);
            configurationStore.Update(newDoc);
        }

        protected abstract TConfiguration MapFromResource(TResource resource);

        public bool GetIsEnabled()
        {
            return GetProperty(doc => doc.IsEnabled);
        }

        public void SetIsEnabled(bool isEnabled)
        {
            SetProperty(doc => doc.IsEnabled = isEnabled);
        }

        protected TProperty GetProperty<TProperty>(Func<TConfiguration, TProperty> prop)
        {
            var doc = configurationStore.Get<TConfiguration>(Id);
            if (doc == null)
                throw new InvalidOperationException($"{ConfigurationSetName} configuration initialization has not executed");

            return prop(doc);
        }

        protected void SetProperty(Action<TConfiguration> callback)
        {
            var doc = configurationStore.Get<TConfiguration>(Id);
            if (doc == null)
                throw new InvalidOperationException($"{ConfigurationSetName} configuration initialization has not executed");

            callback(doc);
            configurationStore.Update(doc);
        }

        public abstract string Id { get; }

        public abstract string ConfigurationSetName { get; }

        public abstract string Description { get; }

        public Type MetadataResourceType => typeof(TResource);

        public abstract IEnumerable<ConfigurationValue> GetConfigurationValues();
    }
}