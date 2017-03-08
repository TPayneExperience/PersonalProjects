using System.Collections.Generic;

namespace Component {
    public class ComponentManager {
        private static ComponentManager _instance;
        Dictionary<string, List<int>> _componentEntities = new Dictionary<string, List<int>>();

        private ComponentManager() { }

        private void KeyCheck(string componentName) {
            if (!_componentEntities.ContainsKey(componentName)) {
                _componentEntities.Add(componentName, new List<int>());
            }
        }
        private bool ValueCheck(string componentName, int id) {
            KeyCheck(componentName);
            return (_componentEntities[componentName].Contains(id));
        }

        public List<int> Get(string componentName) {
            KeyCheck(componentName);
            return _componentEntities[componentName];
        }
        public static ComponentManager Instance {
            get {
                if (_instance == null) {
                    _instance = new ComponentManager();
                }
                return _instance;
            }
        }
        public void Add(string componentName, int id) {
            KeyCheck(componentName);
            _componentEntities[componentName].Add(id);
        }
        public void Remove(string componentName, int id) {
            if (ValueCheck(componentName, id))
                _componentEntities[componentName].Remove(id);
        }

    }
}
