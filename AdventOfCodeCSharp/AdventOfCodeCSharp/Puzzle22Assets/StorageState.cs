using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle22Assets
{
    /// <summary>
    /// Wrapper around a dictionary of storage states
    /// </summary>
    public class StorageState: IEquatable<StorageState>, IComparable
    {

        private static int _stateID = 0;
        
        public override int GetHashCode()
        {
            return _stateID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is StorageState)
            {
                return ((StorageState)obj).ID == ID;
            } else
                return base.Equals(obj);
        }

        public int ID { get; set; }

        public int DesiredDataOnX { get; set; }
        public int DesiredDataOnY { get; set; }

        private List<StorageNode> _nodes;
        private Dictionary<string, int> _storageState;

        private Queue<Tuple<int, int>> _openNodeWayPoints;

        public Queue<Tuple<int, int>> OpenNodeWayPoints { get { return _openNodeWayPoints; } }
        

        public StorageState(List<StorageNode> nodes)
        {
            ID = ++_stateID;
            _nodes = nodes;
            _storageState = new Dictionary<string, int>();
            _openNodeWayPoints = new Queue<Tuple<int, int>>();
        }

        public Dictionary<string, int> State { get { return _storageState; } }

        private string _hashCalced = null;

        public string HashState()
        {
            if (_hashCalced != null)
                return _hashCalced;

            StringBuilder state = new StringBuilder();
            state.Append(DesiredDataOnX);
            state.Append("-");
            state.Append(DesiredDataOnY);
            List<string> keys = new List<String>(_storageState.Keys);
            keys.Sort();
            foreach (string key in keys)
            {
                StorageNode hashNode = _nodes.First(n => n.NodeName == key);
                int nodeRepresentation = hashNode.X * 100 + hashNode.Y;
                string hash = nodeRepresentation.ToString() + "-" + _storageState[key];
                state.Append(hash);
            }
            _hashCalced = state.ToString().GetHashCode().ToString();
            return _hashCalced;
        }

        public bool Equals(StorageState other)
        {
            return other.ID == ID;
        }

        public int CompareTo(object obj)
        {
            if (obj is StorageState)
            {
                return ID.CompareTo(((StorageState)obj).ID);
            }
            else
            {
                return -1;
            }
        }

        public Tuple<int, int> CurrentOpenNodeWayPoint(out bool goalNode, out int depth)
        {
            if (OpenNodeWayPoints.Count == 0)
            {
                goalNode = true;
                depth = 0;
                return new Tuple<int, int>(DesiredDataOnX, DesiredDataOnY);
            }
            goalNode = false;
            depth = OpenNodeWayPoints.Count;
            return OpenNodeWayPoints.Peek();
        }

        public void HitWayPoint()
        {
            _openNodeWayPoints.Dequeue();
        }

        public StorageState Clone()
        {
            StorageState result = new StorageState(_nodes);
            result.DesiredDataOnX = DesiredDataOnX;
            result.DesiredDataOnY = DesiredDataOnY;
            foreach (var n in _openNodeWayPoints)
            {
                result.OpenNodeWayPoints.Enqueue(n);
            }

            foreach(string s in _storageState.Keys)
            {
                result._storageState[s] = _storageState[s];
            }
            return result;
        }
    }
}
