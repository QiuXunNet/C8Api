using System;
using System.Collections.Generic;
using System.Linq;

namespace Qiuxun.C8.Api.Service.Common.KeyWordsFilter
{


    public class KeyWordFilter
    {
        private FilterKeyWords _filter;

        public KeyWordFilter()
        {
        }

        public KeyWordFilter(IList<string> keyWordList)
        {
            if (keyWordList == null)
            {
                throw new Exception("传入的keyWordList对象为空");
            }
            string[] keyWords = keyWordList.ToArray<string>();
            for (int i = 0; i < keyWords.Length; i++)
            {
                if (!string.IsNullOrEmpty(keyWords[i]))
                {
                    keyWords[i] = keyWords[i].Trim();
                }
            }
            this._filter = new FilterKeyWords(keyWords);
        }

        public bool Contains(string sourceContent)
        {
            if (this._filter == null)
            {
                throw new Exception("请先初始化keyWordList词库");
            }
            Dictionary<int, int> dictionary = this._filter.Find(sourceContent);
            return ((dictionary != null) && (dictionary.Count > 0));
        }

        public Dictionary<int, int> Find(string sourceContent)
        {
            if (this._filter == null)
            {
                throw new Exception("请先初始化keyWordList词库");
            }
            return this._filter.Find(sourceContent);
        }

        public Dictionary<int, int> Find(IList<string> keyWordList, string sourceContent)
        {
            if (keyWordList == null)
            {
                throw new Exception("传入的keyWordList对象为空");
            }
            string[] keyWords = keyWordList.ToArray<string>();
            for (int i = 0; i < keyWords.Length; i++)
            {
                if (!string.IsNullOrEmpty(keyWords[i]))
                {
                    keyWords[i] = keyWords[i].Trim();
                }
            }
            this._filter = new FilterKeyWords(keyWords);
            return this._filter.Find(sourceContent);
        }
    }
}

