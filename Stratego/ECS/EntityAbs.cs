using System.Collections.Generic;

namespace Entity {
    public class EntityAbs {
        private int _id;
        public Dictionary<string, Component.ComponentAbs> components = new Dictionary<string, Component.ComponentAbs>();

        protected EntityAbs() { _id = EntityManager.Instance.Add(this); }
        
        public int Id {
            get { return _id; }
        }
        public void Add(Component.ComponentAbs c) {
            components[c.Name] = c;
        }
        public void Remove(string name) {
            components.Remove(name);
            Component.ComponentManager.Instance.Remove(name, _id);
        }
    }
}
