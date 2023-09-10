using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.LevelDesign.Generation.Grammar
{
    public class GrammarGenerator : MonoBehaviour
    {
        public List<Rule> rules;

        [SerializeField] Rule startRule;

        //private void Start()
        //{
        //    Generate();
        //}

        public void Generate()
        {
            List<Symbol> symbols = new List<Symbol>(startRule.GetComponentsInChildren<Symbol>());
            Symbol entranceSymbol = null;
            foreach (Symbol s in symbols)
            {
                if (s.initialized) continue;

                s.SetParents();
            }

            while (true)
            {
                foreach (Symbol s in symbols)
                {
                    if (s.symbol == "e")
                        entranceSymbol = s;
                }

                symbols.Clear();
                symbols = GetMap(entranceSymbol);

                List<Symbol> nonTerminalSymbols = symbols.Where(s => !s.terminal).ToList();

                if (nonTerminalSymbols.Count > 0)
                {
                    Symbol s = nonTerminalSymbols[0];
                    nonTerminalSymbols.RemoveAt(0);
                    if (s == null) continue;

                    ReplaceCallback callback = null;

                    if (s.childs.Count > 0)
                    {
                        foreach (Symbol child in s.childs)
                        {
                            List<Rule> matches = new List<Rule>();
                            foreach (Rule r in rules)
                            {
                                if (r.startSymbol.symbol == s.symbol)
                                {
                                    if (r.endSymbol != null)
                                    {
                                        if (r.endSymbol.symbol == child.symbol)
                                        {
                                            matches.Add(r);
                                        }
                                    }
                                }
                            }

                            if (matches.Count > 0)
                            {
                                callback = new ReplaceCallback(child, matches[Random.Range(0, matches.Count)]);
                                break;
                            }
                        }
                    }
                    else
                    {
                        List<Rule> match = rules.Where(r => r.startSymbol.symbol == s.symbol && r.endSymbol == null).ToList();
                        if (match.Count > 0)
                        {
                            callback = new ReplaceCallback(null, match[Random.Range(0, match.Count)]);
                        }
                    }

                    if (callback == null)
                    {
                        List<Rule> matches = rules.Where(r => r.startSymbol.symbol == s.symbol && r.endSymbol == null).ToList();
                        if (matches.Count > 0)
                        {
                            callback = new ReplaceCallback(null, matches[Random.Range(0, matches.Count)]);
                        }
                    }

                    if (callback != null)
                    {
                        Rule r = s.ReplaceChild(callback);
                        r.transform.parent = startRule.transform;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public void GenerateStep()
        {
            List<Symbol> symbols = new List<Symbol>(startRule.GetComponentsInChildren<Symbol>());
            Symbol entranceSymbol = null;
            foreach (Symbol s in symbols)
            {
                if (s.symbol == "e")
                    entranceSymbol = s;

                if (s.initialized) continue;

                s.SetParents();
            }

            symbols.Clear();
            symbols = GetMap(entranceSymbol);

            List<Symbol> nonTerminalSymbols = symbols.Where(s => !s.terminal).ToList();

            if (nonTerminalSymbols.Count > 0)
            {
                Symbol s = nonTerminalSymbols[0];
                nonTerminalSymbols.RemoveAt(0);

                ReplaceCallback callback = null;

                if (s.childs.Count > 0)
                {
                    foreach (Symbol child in s.childs)
                    {
                        List<Rule> matches = new List<Rule>();
                        foreach (Rule r in rules)
                        {
                            if (r.startSymbol.symbol == s.symbol)
                            {
                                if (r.endSymbol != null)
                                {
                                    if (r.endSymbol.symbol == child.symbol)
                                    {
                                        matches.Add(r);
                                    }
                                }
                            }
                        }

                        if (matches.Count > 0)
                        {
                            callback = new ReplaceCallback(child, matches[Random.Range(0, matches.Count)]);
                            break;
                        }
                    }

                    {
                        //for (int i = 0; i < s.childs.Count; i++)
                        //{
                        //    List<Rule> match = new List<Rule>();
                        //    foreach (Rule rule in rules)
                        //    {
                        //        if (rule.startSymbol.symbol == s.symbol)
                        //        {
                        //            if (rule.endSymbol != null)
                        //            {
                        //                if (rule.endSymbol.symbol == s.childs[i].symbol)
                        //                    match.Add(rule);
                        //            }
                        //        }
                        //    }

                        //    if (match.Count > 0)
                        //    {
                        //        callback = new ReplaceCallback(i, match[Random.Range(0, match.Count)]);
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        match = rules.Where(r => r.startSymbol.symbol == s.symbol && r.endSymbol == null).ToList();
                        //        if (match.Count > 0)
                        //        {
                        //            callback = new ReplaceCallback(i, match[Random.Range(0, match.Count)]);
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
                else
                {
                    List<Rule> match = rules.Where(r => r.startSymbol.symbol == s.symbol && r.endSymbol == null).ToList();
                    if (match.Count > 0)
                    {
                        callback = new ReplaceCallback(null, match[Random.Range(0, match.Count)]);
                    }
                }

                if (callback == null)
                {
                    List<Rule> matches = rules.Where(r => r.startSymbol.symbol == s.symbol && r.endSymbol == null).ToList();
                    if (matches.Count > 0)
                    {
                        callback = new ReplaceCallback(null, matches[Random.Range(0, matches.Count)]);
                    }
                }

                if (callback != null)
                {
                    Rule r = s.ReplaceChild(callback);
                    r.transform.parent = startRule.transform;
                    //nonTerminalSymbols.AddRange(r.GetComponentsInChildren<Symbol>().Where(sy => !sy.terminal));
                }
            }
        }

        List<Symbol> GetMap(Symbol entrance)
        {
            List<Symbol> map = new List<Symbol>();
            Queue<Symbol> next = new Queue<Symbol>();
            next.Enqueue(entrance);

            while(next.Count > 0)
            {
                Symbol currentSymbol = next.Dequeue();

                if (currentSymbol == null) continue;

                if(!map.Contains(currentSymbol))
                    map.Add(currentSymbol);

                foreach(Symbol s in currentSymbol.childs)
                {
                    next.Enqueue(s);
                }
            }

            return map;
        }
    }

    public class ReplaceCallback
    {
        public readonly Symbol child;
        public readonly Rule rule;

        public ReplaceCallback(Symbol child, Rule rule)
        {
            this.child = child;
            this.rule = rule;
        }
    }
}
