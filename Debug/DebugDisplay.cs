using Godot;
using System;
using System.Collections.Generic;

namespace RadPipe.Debug
{
    public partial class DebugDisplay : Control
    {
        [Export] BoxContainer BaseBoxContainer = new BoxContainer();

        private List<DebugInterfaceItem> _items = new List<DebugInterfaceItem>();
        public override void _EnterTree()
        {
            RadDebug.Initialise(this);
        }
        public override void _PhysicsProcess(double delta)
        {
            if (Input.IsActionJustPressed("dev_button"))
            {
                Visible = !Visible;
            }
        }

        public void SetDebugProperty(string tag, string value)
        {
            foreach (var item in _items)
            {
                if (item.GetTag() == tag)
                {
                    UpdateProperty(item, value);
                    return;
                }
            }
            var db = new DebugInterfaceItem(tag, value);
            _items.Add(db);
            BaseBoxContainer.AddChild(db.GetAsContainerNode());
        }
        public void UpdateProperty(DebugInterfaceItem Item, string value)
        {
            Item.UpdateValue(value);
        }

        public void RemoveItem(string tag)
        {
            foreach (var item in _items)
            {
                if (item.GetTag() == tag)
                {
                    item.Destroy();
                    _items.Remove(item);
                    return;
                }
            }
        }

        public class DebugInterfaceItem
        {
            string tag;
            BoxContainer contianer;
            Label lLabel;
            Label lValue;

            public DebugInterfaceItem(string tag, string value)
            {
                this.tag = tag;
                contianer = new BoxContainer();
                lLabel = new Label();
                lValue = new Label();
                lLabel.Text = tag;
                lValue.Text = value;
                contianer.AddChild(lLabel);
                contianer.AddChild(lValue);
            }

            public BoxContainer GetAsContainerNode()
            {
                return contianer;
            }

            public void UpdateValue(string value)
            {
                lValue.Text = value;
            }

            public string GetTag() => tag;

            public void Destroy()
            {
                contianer.QueueFree();
            }
        }
    }
}
