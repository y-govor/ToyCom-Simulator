using System.Collections;
using System.Collections.Generic;

namespace ToyCom.Utilities
{
    public class Languages : IEnumerable<string>
    {
        // Available languages
        private Dictionary<string, Language> _langs;

        // Elements
        public int Count { get; private set; }

        // Constructor
        public Languages()
        {
            // Available languages
            _langs = new Dictionary<string, Language>()
            {
                { "English", Language.English },
                { "Ukrainian", Language.Ukrainian }
            };

            Count = _langs.Count;
        }

        public Languages(Dictionary<string, Language> langs)
        {
            // Available languages
            _langs = langs;
            Count = _langs.Count;
        }

        // Class indexers
        public Language this[string key]
        {
            get { return _langs[key]; }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _langs.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}