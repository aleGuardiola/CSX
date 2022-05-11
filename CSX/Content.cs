using CSX.Components;
using System.Collections;

namespace CSX
{
    public class Content : IEnumerable<Element>
    {
        List<Element> internalList = new List<Element>();
        public IEnumerator<Element> GetEnumerator() => internalList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => internalList.GetEnumerator();

        public Content() { }

        public Content(IEnumerable<Element> views)
        {
            foreach (var view in views)
            {
                Add(view);
            }
        }

        public Content Add(Element element)
        {
            if (element == null)
            {
                return this;
            }
            internalList.Add(element);
            return this;
        }

        public Content Add(Content elements)
        {
            if (elements.internalList.Count == 0)
            {
                return this;
            }

            internalList.AddRange(elements.Where(e => e != null));
            return this;
        }
    }


    public static class EnumerableExtensions
    {        
        public static Content ToContent(this IEnumerable<Element> views)
            => new Content(views);
    }
}
