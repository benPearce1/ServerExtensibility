﻿using System;

namespace Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration
{
    public class ConfigureCommandOption
    {

        public ConfigureCommandOption(string prototype, string description, Action<string> action)
            // ReSharper disable once IntroduceOptionalParameters.Global - this ctor is part of the public interface
            :this (prototype, description, action, hide: false)
        {
            
        }

        public ConfigureCommandOption(string prototype, string description, Action<string> action, bool hide)
            // ReSharper disable once IntroduceOptionalParameters.Global - this ctor is part of the public interface
            : this(prototype, description, action, hide: false, requiresRestart: false)
        {

        }

        public ConfigureCommandOption(string prototype, string description, Action<string> action, bool hide, bool requiresRestart)
        {
            Prototype = prototype;
            Description = description;
            Action = action;
            Hide = hide;
            RequiresRestart = requiresRestart;
        }

        public string Prototype { get; set; }

        public string Description { get; set; }

        public Action<string> Action { get; set; }

        public bool Hide { get; set; }

        public bool RequiresRestart { get; set; }
    }
}