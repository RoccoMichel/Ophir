using Photon.Pun;

public class ServerPlayer : BasePlayer // Very much unfinished
{
    public int score;
    public string nickname = "annon";

    private void Start()
    {
        try { nickname = PhotonNetwork.NickName; }
        catch { }
    }

    public virtual void AddScore(int amount)
    {
        score += amount;
    }

    public override void Die()
    {
        base.Die();
        AddScore(1); // to the killer idk who that was
    }
}