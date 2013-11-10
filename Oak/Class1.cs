using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class Oak //manager
    {
        Tree Collection;
        int count;
        public Oak()
        {
            Collection = new Tree();
        }

        public string ProcessLine(string _line)
        {
            List<string> words = System.Text.RegularExpressions.Regex.Replace( _line,@"\s+"," ").Trim().Split(' ').ToList<string>();
            List<Leaf> newleafs = new List<Leaf>();
            //Task.Factory.StartNew(() => Parallel.ForEach<string>(words, word => { newleafs.Add(process(word, words)); }));
            foreach (string _word in words)
            {
                if (!String.IsNullOrEmpty(_word))
                {
                    Leaf newleaf = new Leaf(_word);
                    if (!((words.IndexOf(_word) + 1) >= words.Count))
                    {
                        newleaf.AddRelation(words.ElementAt(words.IndexOf(_word) + 1));
                    }
                    newleafs.Add(newleaf);
                }
            }

            foreach(Leaf leaf in newleafs)
            {
                if (!Collection.Data.ContainsKey(leaf.Word))
                {
                    Collection.AddLeaf(leaf);
                }
                else
                {
                    if(leaf.Relations.Count != 0)
                    Collection.Data[leaf.Word].UpdateRelation(leaf.Relations.ElementAt(0).Key);
                }
            }
            return answer(words);
        }

        string answer(List<string> _words)
        {
            Tree dispCollection = new Tree(Collection.DumpData());
            string word = _words.ElementAt(new Random(DateTime.Now.Millisecond).Next(0, _words.Count-1));
            count = 0;
            return Cap(EvalNode(word, dispCollection).Trim() + ".");
        }

        string EvalNode(string _word ,Tree _col)
        {

            Dictionary<string, MagPair> relations = _col.Data[_word].Relations;
            if (relations.Count != 0)
            {
                float max = relations.Values.Max(val => val.Magnitude);
                if (max > 0.3)
                {
                    if (count <= 20)
                    {
                        count++;
                        string nodeid = relations.FirstOrDefault(x => x.Value.Magnitude == max).Key;
                        _col.Data[_word].Relations.Remove(nodeid);
                        return nodeid + " " + EvalNode(nodeid, _col);
                    }
                    else
                    {
                        return "..";
                    }
                }
            }
            return "";      
        }

        static string Cap(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }


    public class Tree //collecton
    {
        private Dictionary<string, Leaf> leafs = new Dictionary<string, Leaf>(); // identifier, relational clusterfuck
        public Tree(){}
        public Tree(Dictionary<string, Leaf> _data)
        {
            leafs = _data;
        }
        public int Count(){ return leafs.Count();}
        public void AddLeaf(Leaf _leaf)
        {
            if (!leafs.ContainsKey(_leaf.Word))
            {
                leafs.Add(_leaf.Word, _leaf);
            }
        }
        public Dictionary<string, Leaf> DumpData(){ return leafs;}
        public Dictionary<string, Leaf> Data { get { return leafs; } }
    }

    public class Leaf //node
    {
        public string Word;
        Dictionary<string, MagPair> relations = new Dictionary<string, MagPair>();
        
        public Leaf(string _word)
        {
            Word = _word;
        }

        public Dictionary<string, MagPair> Relations
        {
            get { return relations; }
        }

        public void AddRelation(string _word)
        {
            if (!relations.ContainsKey(_word))
            {
                relations.Add(_word, new MagPair(0, 1));
                relations[_word].Magnitude = relations[_word].Count / relations.Values.Sum(magpair => magpair.Count);
            }
        }

        public void UpdateRelation(string _word)
        {
            if (relations.ContainsKey(_word))
            {
                relations[_word].Count++;
                relations[_word].Magnitude = (float)relations[_word].Count / (float)relations.Values.Sum(magpair => (float)magpair.Count); // needs fixing. mag stays at zero
            }
            else
            {
                relations.Add(_word, new MagPair(0, 1));
                relations[_word].Magnitude = (float)relations[_word].Count / (float)relations.Values.Sum(magpair => (float)magpair.Count);
            }
        }
    }

    public class MagPair
    {
        public float Magnitude { get; set; }
        public int Count { get; set; }
        public MagPair() { }
        public MagPair(float _magnitude, int _count)
        {
            Magnitude = _magnitude;
            Count = _count;
        }
    }
}
