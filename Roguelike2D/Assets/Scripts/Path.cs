public class Path : object
{
    public int g;         
    public int h;         
    public Path parent;   
    public int x;         
    public int y;         

    public Path(int _g, int _h, Path _parent, int _x, int _y)
    {
        g = _g;
        h = _h;
        parent = _parent;
        x = _x;
        y = _y;
    }

    public int F
    {
        get
        {
            return g + h;
        }
    }
}