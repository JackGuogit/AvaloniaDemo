using System;
using System.Collections.Generic;

namespace AvaloniaPrsimSimple.ViewModels;

public partial class DragDropViewModel:ViewModelBase
{
    
    private List<ItemCoordinates> _consumableList;

    public List<ItemCoordinates> ConsumableList
    {
        get => _consumableList;
        set
        {
            if (Equals(value, _consumableList)) return;
            _consumableList = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged(nameof(ConsumableList));
        }
    }

    public List<ItemCoordinates> PlatePositionList
    {
        get => _platePositionList;
        set
        {
            if (Equals(value, _platePositionList)) return;
            _platePositionList = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged(nameof(PlatePositionList));
        }
    }

    private List<ItemCoordinates> _platePositionList;


    public DragDropViewModel()
    {
        
    }
    
}
    
public class ItemCoordinates
{
    public int Index { get; }
    public double DistanceToTop { get; }

    public ItemCoordinates(int index, double distanceToTop)
    {
        Index = index;
        DistanceToTop = distanceToTop;
    }
}