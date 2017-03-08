namespace Component {
    public class ComponentAbs {
        string _name;
        int _ownerId;

        protected ComponentAbs(string name, int id) {
            ComponentManager.Instance.Add(name, id);
            _name = name;
            _ownerId = id;
        }
        public string Name {
            get {
                return _name;
            }
        }
    }
}
