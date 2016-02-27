using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PscdPack
{
    class LzTree
    {
        byte[] dict;
        int[] rightChildren;
        int[] leftChildren;
        int[] parents;
        readonly int F;
        readonly int N;
        readonly int NIL;

        public int MatchPos { get; private set; }
        public int MatchLength { get; set; }

        public LzTree(byte[] dict, int F)
        {
            this.dict = dict;
            this.F = F;
            N = dict.Length - F + 1;
            NIL = N;
            Reset();
        }

        public void Reset()
        {
            rightChildren = new int[N + 1 + 256];
            for (int i = N + 1; i <= N + 256; ++i) rightChildren[i] = NIL;
            leftChildren = new int[N + 1];
            parents = new int[N + 1];
            for (int i = 0; i < N; ++i) parents[i] = NIL;
        }

        public void InsertNode(int pos)
        {
            int cmp = 1;
            int p = N + 1 + dict[pos];
            leftChildren[pos] = NIL;
            rightChildren[pos] = NIL;
            MatchLength = 0;

            while (true)
            {
                if (cmp >= 0)
                {
                    if (rightChildren[p] != NIL)
                    {
                        p = rightChildren[p];
                    }
                    else
                    {
                        rightChildren[p] = pos;
                        parents[pos] = p;
                        return;
                    }
                }
                else
                {
                    if (leftChildren[p] != NIL)
                    {
                        p = leftChildren[p];
                    }
                    else
                    {
                        leftChildren[p] = pos;
                        parents[pos] = p;
                        return;
                    }
                }

                int i;
                for (i = 1; i < F; ++i)
                {
                    cmp = dict[pos + i] - dict[p + i];
                    if (cmp != 0) break;
                }
                if (i > MatchLength)
                {
                    MatchPos = p;
                    MatchLength = i;
                    if (i >= F)
                    {
                        parents[pos] = parents[p];
                        leftChildren[pos] = leftChildren[p];
                        rightChildren[pos] = rightChildren[p];
                        parents[leftChildren[p]] = pos;
                        parents[rightChildren[p]] = pos;
                        if (rightChildren[parents[p]] == p)
                        {
                            rightChildren[parents[p]] = pos;
                        }
                        else
                        {
                            leftChildren[parents[p]] = pos;
                        }
                        parents[p] = NIL;
                        return;
                    }
                }
            }
        }

        public void DeleteNode(int node)
        {
            if (parents[node] == NIL) return;

            int q;
            if (rightChildren[node] == NIL)
            {
                q = leftChildren[node];
            }
            else if (leftChildren[node] == NIL)
            {
                q = rightChildren[node];
            }
            else
            {
                q = leftChildren[node];
                if (rightChildren[q] != NIL)
                {
                    do
                    {
                        q = rightChildren[q];
                    } while (rightChildren[q] != NIL);
                    rightChildren[parents[q]] = leftChildren[q];
                    parents[leftChildren[q]] = parents[q];
                    leftChildren[q] = leftChildren[node];
                    parents[leftChildren[node]] = q;
                }
                rightChildren[q] = rightChildren[node];
                parents[rightChildren[node]] = q;
            }
            parents[q] = parents[node];
            if (rightChildren[parents[node]] == node)
            {
                rightChildren[parents[node]] = q;
            }
            else
            {
                leftChildren[parents[node]] = q;
            }
            parents[node] = NIL;
        }
    }
}
