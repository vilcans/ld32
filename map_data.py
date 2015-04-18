import json

data = json.load(open('map/map.json'))

layer = data['layers'][0]

width = layer['width']
height = layer['height']
layer_data = layer['data']

with open('Assets/MapData.cs', 'w') as out:
    out.write("""
public class MapData {
    public int width = %s;
    public int height = %s;
    public byte[] tileData = {%s};
}""" % (width, height, ','.join(str(v - 1) for v in layer_data))
)
