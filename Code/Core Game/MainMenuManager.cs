using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public GoblinKing goblinKing;
    public GiantSkeleton giantSkeleton;
    public KingGhost kingGhost;
    public DragonBoss dragonBoss;

    [Header("Goblin stats")]
    public TMP_Text goblinHealth;
    public TMP_Text goblinAttack;
    public TMP_Text goblinPower;
    public TMP_Text goblinLeadership;

    [Header("Giant Skeleton Stats")]
    public TMP_Text giantHealth;
    public TMP_Text giantAttack;
    public TMP_Text giantPower;
    public TMP_Text giantLeadership;

    [Header("King Ghost Stats")]
    public TMP_Text ghostHealth;
    public TMP_Text ghostAttack;
    public TMP_Text ghostPower;
    public TMP_Text ghostLeadership;

    [Header("Dragon Stats")]
    public TMP_Text dragonHealth;
    public TMP_Text dragonAttack;
    public TMP_Text dragonPower;
    public TMP_Text dragonLeadership;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        goblinHealth.text = goblinKing.health.ToString();
        goblinAttack.text = goblinKing.attackDamage.ToString(); 
        goblinPower.text = goblinKing.powerValue.ToString();
        goblinLeadership.text = goblinKing.leadershipValue.ToString();

        giantHealth.text = giantSkeleton.health.ToString();
        giantAttack.text = giantSkeleton.attackDamage.ToString();
        giantPower.text = giantSkeleton.powerValue.ToString();
        giantLeadership.text = giantSkeleton.leadershipValue.ToString();

        ghostHealth.text = kingGhost.health.ToString();
        ghostAttack.text = kingGhost.attackDamage.ToString();
        ghostPower.text = kingGhost.powerValue.ToString();
        ghostLeadership.text = kingGhost.leadershipValue.ToString();

        dragonHealth.text = dragonBoss.health.ToString();
        dragonAttack.text = dragonBoss.attackDamage.ToString();
        dragonPower.text = dragonBoss.powerValue.ToString();
        dragonLeadership.text = dragonBoss.leadershipValue.ToString();
    }
}
