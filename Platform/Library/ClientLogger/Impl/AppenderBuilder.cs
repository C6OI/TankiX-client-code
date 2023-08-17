using System;
using System.Collections.Generic;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;

namespace Platform.Library.ClientLogger.Impl {
    public abstract class AppenderBuilder {
        const string DEFAULT_LAYOUT = "%d{ABSOLUTE} %5p %c - %m%n";

        readonly List<IFilter> excludeFilters = new();

        readonly List<IFilter> includeFilters = new();

        AppenderSkeleton appender;

        protected void Init(AppenderSkeleton appender) => this.appender = appender;

        public AppenderBuilder SetName(string appenderName) {
            appender.Name = appenderName;
            return this;
        }

        public AppenderBuilder DenyAllFilter() {
            includeFilters.Add(new DenyAllFilter());
            return this;
        }

        public AppenderBuilder ClearFilters() {
            appender.ClearFilters();
            return this;
        }

        public AppenderBuilder AddLoggerMatchFilter(string matchString) {
            AddLoggerMatchFilter(matchString, true);
            return this;
        }

        public AppenderBuilder AddLoggerMatchFilter(Type matchType) => AddLoggerMatchFilter(matchType.FullName);

        public AppenderBuilder AddLoggerMatchExculdeFilter(string matchString) {
            AddLoggerMatchFilter(matchString, false);
            return this;
        }

        public AppenderBuilder AddLoggerMatchExculdeFilter(Type matchType) {
            AddLoggerMatchExculdeFilter(matchType.FullName);
            return this;
        }

        void AddLoggerMatchFilter(string matchString, bool include) {
            LoggerMatchFilter loggerMatchFilter = new();
            loggerMatchFilter.LoggerToMatch = matchString;
            loggerMatchFilter.AcceptOnMatch = include;

            if (include) {
                AddIncludeFilter(loggerMatchFilter);
            } else {
                AddExcludeFilter(loggerMatchFilter);
            }
        }

        public AppenderBuilder AddExcludeFilter(IFilter filter) {
            excludeFilters.Add(filter);
            return this;
        }

        public AppenderBuilder AddIncludeFilter(IFilter filter) {
            includeFilters.Add(filter);
            return this;
        }

        public AppenderBuilder SetLevel(Level level) {
            appender.Threshold = level;
            return this;
        }

        public AppenderBuilder SetLayout(ILayout layout) {
            appender.Layout = layout;
            return this;
        }

        public AppenderBuilder SetLayout(string layoutString) {
            PatternLayout layout = new(layoutString);
            return SetLayout(layout);
        }

        public AppenderSkeleton Configure() {
            if (appender.Layout == null) {
                SetLayout("%d{ABSOLUTE} %5p %c - %m%n");
            }

            foreach (IFilter excludeFilter in excludeFilters) {
                appender.AddFilter(excludeFilter);
            }

            foreach (IFilter includeFilter in includeFilters) {
                appender.AddFilter(includeFilter);
            }

            if (includeFilters.Count > 0) {
                appender.AddFilter(new DenyAllFilter());
            }

            return appender;
        }
    }
}