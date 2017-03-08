using System.Collections.Generic;

namespace Entity {
    public class EntityManager {
        private Dictionary<int, EntityAbs> _entities = new Dictionary<int, EntityAbs>();
        private static EntityManager _instance;
        private int nextId = 0;

        private EntityManager() { }
        public static EntityManager Instance {
            get {
                if (_instance == null) {
                    _instance = new EntityManager();
                }
                return _instance;
            }
        }

        public EntityAbs Get(int id) {
            System.Diagnostics.Debug.Assert(_entities.ContainsKey(id));
            return _entities[id];
        }
        public int Add(EntityAbs e) {
            _entities[nextId] = e;
            return nextId++;
        }
        public void Remove(int id) {
            _entities.Remove(id);
        }
    }
}
