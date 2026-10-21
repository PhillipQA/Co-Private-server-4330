namespace Conquer.Game.Models;

public partial class Player
{
    private const uint FacePrice = 500;

    public int Body => (int)(Model % 1000);

    public bool ChangeAvatar(ushort avatar)
    {
        if (!Spend(FacePrice))
        {
            Tell("You do not have enough money.");
            return false;
        }

        Avatar = avatar;
        return true;
    }

    public void SetHair(ushort hair)
    {
        Hair = hair;
        Show(AttributeType.Hair, Hair);
    }

    public void DyeHair(int color) => SetHair((ushort)(Hair - Hair % 1000 / 100 * 100 + color * 100));

    public void SetBody(int body)
    {
        Model = Model - Model % 1000 + (uint)body;
        Show(AttributeType.Look, Look);
    }
}
