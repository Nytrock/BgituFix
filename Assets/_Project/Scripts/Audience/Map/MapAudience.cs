using System.Collections.Generic;

public class MapAudience : MapElement<EditableComputer, ComputerData, AudienceData> {
    protected override IEnumerable<ComputerData> GetEditablesData(MapData mapData) {
        return mapData.GetComputersByAudience(_data);
    }
}
