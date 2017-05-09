using System;

namespace Plugin
{
    public class Variable<T> : IAccessible, ICloneable
    {
        private T m_Data;
        AccessModifier m_AccessModifier;
        public AccessModifier AccessModifier { get { return m_AccessModifier; }  set { m_AccessModifier = value; } }

        public Variable(T Data)
        {
            m_Data = Data;
        }

        public object Clone()
        {
            if (m_Data.GetType().IsAssignableFrom(typeof(ICloneable)))
                return ((ICloneable)m_Data).Clone();
            return m_Data;
        }
    }
}
