[System.Serializable]
public class Item
{
    public string name;
    public int grade;
    public bool equiped;

    public Item(string nameConstructer, int gradeConstructer, bool equipedConstructer)
    {
        this.name = nameConstructer;
        this.grade = gradeConstructer;
        this.equiped = equipedConstructer;
    }
}
