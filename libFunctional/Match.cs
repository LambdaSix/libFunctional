using System;
using System.Collections.Generic;

namespace libFunctional
{
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(string message) : base(message) { }
    }

    public class PatternMatchContext
    {
        private readonly dynamic _value;

        internal PatternMatchContext(dynamic value)
        {
            _value = value;
        }

        public PatternMatch With(Func<dynamic, bool> condition, Func<dynamic, dynamic> result)
        {
            var match = new PatternMatch(_value);
            return match.With(condition, result);
        }
    }

    public static class PatternMatchExtensions
    {
        public static PatternMatchContext Match<T>(this T value)
        {
            return new PatternMatchContext(value);
        }
    }

    public class PatternMatch
    {
        private readonly dynamic _value;

        private readonly List<Tuple<Func<dynamic, bool>, Func<dynamic, dynamic>>> cases
            = new List<Tuple<Func<dynamic, bool>, Func<dynamic, dynamic>>>();

        private Func<dynamic, dynamic> elseFunc;

        internal PatternMatch(dynamic value)
        {
            _value = value;

        }

        public PatternMatch With(Func<dynamic, bool> condition, Func<dynamic, dynamic> result)
        {
            cases.Add(Tuple.Create(condition, result));
            return this;
        }

        public PatternMatch Else(Func<dynamic, dynamic> result)
        {
            if (elseFunc != null)
            {
                throw new InvalidOperationException("Multiple else conditions specified");
            }
            elseFunc = result;
            return this;
        }

        public dynamic Do()
        {
            if (elseFunc != null)
            {
                cases.Add(Tuple.Create<Func<dynamic, bool>, Func<dynamic, dynamic>>(x => true, elseFunc));
            }

            foreach (var item in cases)
            {
                if (item.Item1(_value))
                {
                    return item.Item2(_value);
                }
            }

            throw new MatchNotFoundException("Non-exhaustive pattern match");
        }
    }
}