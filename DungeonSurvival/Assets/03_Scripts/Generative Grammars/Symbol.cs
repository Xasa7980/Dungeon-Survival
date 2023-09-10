using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.LevelDesign.Generation.Grammar
{
    public class Symbol : MonoBehaviour
    {
        public string symbol;
        public bool terminal;
        public List<Symbol> childs = new List<Symbol>();
        List<Symbol> parents = new List<Symbol>();

        //[SerializeField] bool replaceChild = false;

        [Space(10)]
        [SerializeField] Shape[] shapes;

        public Shape GetShape()
        {
            return shapes[Random.Range(0, shapes.Length)].CreateInstance(this);
        }

        public bool initialized { get; private set; }

        public void SetParents()
        {
            foreach (Symbol s in childs)
            {
                s.parents.Add(this);
            }

            initialized = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            foreach (Symbol s in childs)
            {
                if (s == null) continue;

                Gizmos.DrawLine(transform.position, s.transform.position);
            }
        }

        public Rule ReplaceChild(ReplaceCallback callback)
        {
            Rule r = Instantiate(callback.rule, transform.position, Quaternion.identity);
            foreach (Symbol c in r.GetComponentsInChildren<Symbol>())
            {
                c.SetParents();
            }

            //Reemplazar el simbolo final
            if (callback.child != null)
            {
                childs.Remove(callback.child);
                callback.child.parents.Remove(this);

                if (r.order.Length > 1)
                {
                    r.order[1].childs.AddRange(callback.child.childs);
                    foreach (Symbol c in callback.child.childs)
                    {
                        c.parents.Remove(callback.child);
                        c.parents.Add(r.order[1]);
                    }
                }

                Destroy(callback.child.gameObject);
            }

            //Reemplazar el simbolo inicial
            Symbol startSymbol = r.order[0];
            startSymbol.parents = new List<Symbol>(parents);
            foreach (Symbol p in parents)
            {
                p.childs.Remove(this);
                p.childs.Add(startSymbol);
            }
            foreach (Symbol c in childs)
            {
                c.parents.Remove(this);
                c.parents.Add(startSymbol);
            }
            startSymbol.childs.AddRange(childs);

            Destroy(gameObject);

            return r;
        }
    }
}
