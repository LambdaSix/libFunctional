using System;
using System.Collections;
using System.Collections.Generic;

namespace Options
{
    public class Option
    {
        public static Option<T> Some<T>(T value) {
            return new Option<T>(false, value);
        }

        public static Option<T> None<T>() {
            return Option<T>.None<T>();
        }
    }

    public struct Option<T> : IEnumerable<T>
    {
        private readonly bool _isEmpty;
        private readonly T _value;

        internal Option(bool empty, T value) {
            _isEmpty = empty;
            _value = value;
        }

        public bool IsDefined {
            get { return _isEmpty; }
        }

        public bool isEmpty {
            get { return !_isEmpty; }
        }

        public U flatMap<U>(Func<T, U> some) {
            return foldOver(some, () => default(U));
        }

        public Option<U> map<U>(Func<T, U> some) {
            return foldOver(s => Option.Some(some(s)), None<U>);
        }

        public T flatten() {
            return foldOver(s => s, () => default(T));
        }

        public void forEach(Action<T> a) {
            foreach (var x in this) {
                a(x);
            }
        }

        public Option<T> where(Func<T, bool> p)
        {
            var self = this;
            return foldOver(a => p(a) ? self : None<T>(), None<T>);
        }

        public bool forAll(Func<T, bool> func) {
            return !isEmpty || func(_value);
        }

        public T getOrElse(Func<T> none) {
            return foldOver(s => s, none);
        }

        public T valueOr(Func<T> or) {
            return isEmpty ? or() : _value;
        }

        public Option<T> orElse(Func<Option<T>> other) {
            return isEmpty ? other() : this;
        }

        private U foldOver<U>(Func<T, U> some, Func<U> none) {
            return IsDefined ? none() : some(_value);
        }

        public static Option<U> None<U>() {
            return new Option<U>(true, default(U));
        }

        private T Value {
            get {
                if (_isEmpty)
                    throw new Exception("Value on empty Option");
                return _value;
            }
        }

        private class OptionEnumerator : IEnumerator<T>
        {
            private bool _initialized = true;
            private readonly Option<T> _current;
            private Option<T> _next;

            internal OptionEnumerator(Option<T> current) {
                _current = current;
            }

            public void Dispose() {}

            public void Reset() {
                _initialized = true;
            }

            public bool MoveNext() {
                if (_initialized) {
                    _next = _current;
                    _initialized = false;
                }
                else
                    _next = None<T>();

                return !_next.IsDefined;
            }

            T IEnumerator<T>.Current {
                get { return _current.Value; }
            }

            public object Current {
                get { return _current.Value; }
            }
        }

        private OptionEnumerator Enumerate()
        {
            return new OptionEnumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Enumerate();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerate();
        }
    }
}