namespace Polybool.Net.Objects
{
    public struct Matcher
    {
        public int Index { get; set; }
        public bool MatchesHead { get; set; }
        public bool MatchesPt1 { get; set; }

        public void Set(int index, bool matchesHead, bool matchesPt1)
        {
            Index = index;
            MatchesHead = matchesHead;
            MatchesPt1 = matchesPt1;
        }
    }
}