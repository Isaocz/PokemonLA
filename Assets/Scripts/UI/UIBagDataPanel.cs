using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagDataPanel : MonoBehaviour
{
    public static UIBagDataPanel InBagData;
    public Text MoneyText;
    public Text StoneText;
    public Text HeartScaleText;
    public Text PPUpText;
    public Text SeedofMasteryText;

    public UICallDescribe SpaceItemUI;
    public GameObject PassiveItemBlockPanel;
    public UICallDescribe PassiveItemBlock;
    public PassiveList passiveList;
    PlayerControler player;
    int PIIndexListPointer = 0;
    private readonly List<UICallDescribe> passiveBlocks = new List<UICallDescribe>();
    private readonly List<int> displayedPassives = new List<int>();
    private Coroutine pendingRefresh;
    public void RefreshConsumedPassives()
    {
        if (isActiveAndEnabled && player != null) RestoreBagDataPanel();
    }




    // Start is called before the first frame update
     private void Awake()
    {
        InBagData = this;

    }


    private void OnEnable()
    {
        player = GameObject.FindObjectOfType<PlayerControler>();
        pendingRefresh = StartCoroutine(RefreshNextFrame());
    }

    private IEnumerator RefreshNextFrame()
    {
        // A frame delay works even when opening the inventory sets timeScale to zero.
        yield return null;
        pendingRefresh = null;
        if (player == null) player = FindObjectOfType<PlayerControler>();
        if (player != null) RestoreBagDataPanel();
    }

    private void OnDisable()
    {
        if (pendingRefresh != null) StopCoroutine(pendingRefresh);
        pendingRefresh = null;
    }

    private void OnDestroy()
    {
        if (InBagData == this) InBagData = null;
    }

    void RestoreBagDataPanel()
    {
        MoneyText.text = player.Money.ToString();
        StoneText.text = player.Stone.ToString();
        HeartScaleText.text = player.HeartScale.ToString();
        PPUpText.text = player.PPUp.ToString();
        SeedofMasteryText.text = player.SeedofMastery.ToString();



        if (player.spaceItem != null) { 
            SpaceItemUI.gameObject.SetActive(true);
            SpaceItemUI.TwoMode = true;
            SpaceItemUI.FirstText = player.spaceItem.GetComponent<Item>().ItemName;
            SpaceItemUI.DescribeText = player.spaceItem.GetComponent<Item>().ItemDescribe;
            SpaceItemUI.GetComponent<Image>().sprite = player.spaceItem.GetComponent<Item>().StoreImage;
        }
        else
        {
            SpaceItemUI.gameObject.SetActive(false);
        }
        PlayerData playerdata = player.playerData;
        // Items can now be consumed. Rebuild our own entries when list contents change.
        bool changed = displayedPassives.Count != playerdata.GetPassiveItemList.Count;
        if (!changed) for (int i = 0; i < displayedPassives.Count; i++)
            if (displayedPassives[i] != playerdata.GetPassiveItemList[i]) { changed = true; break; }
        if (changed)
        {
            foreach (var block in passiveBlocks) if (block != null) { block.gameObject.SetActive(false); Destroy(block.gameObject); }
            passiveBlocks.Clear(); displayedPassives.Clear();
            displayedPassives.AddRange(playerdata.GetPassiveItemList);
            PIIndexListPointer = 0;
        }
        for ( ; PIIndexListPointer < playerdata.GetPassiveItemList.Count ; PIIndexListPointer++)
        {
            UICallDescribe Block =  Instantiate(PassiveItemBlock, PassiveItemBlockPanel.transform.position, Quaternion.identity, PassiveItemBlockPanel.transform);
            passiveBlocks.Add(Block);
            Block.TwoMode = true;
            Block.FirstText = passiveList.transform.GetChild(playerdata.GetPassiveItemList[PIIndexListPointer]).GetComponent<PassiveItem>().ItemName;
            Block.DescribeText = passiveList.transform.GetChild(playerdata.GetPassiveItemList[PIIndexListPointer]).GetComponent<PassiveItem>().ItemDescribe;
            Image icon = Block.transform.GetChild(0).GetComponent<Image>();
            int itemId = playerdata.GetPassiveItemList[PIIndexListPointer];
            Sprite original = passiveList.transform.GetChild(itemId).GetComponent<SpriteRenderer>().sprite;
            icon.sprite = original;
            var animation = PassiveIconAnimationSet.Find(itemId);
            if (animation != null)
            {
                var playback = icon.GetComponent<UIItemFrameAnimation>();
                if (playback == null) playback = icon.gameObject.AddComponent<UIItemFrameAnimation>();
                playback.Bind(original, animation);
            }
            Block.DescribeUI = SpaceItemUI.DescribeUI;
        }
        RebuildPassiveLayout();
    }

    private void RebuildPassiveLayout()
    {
        // The grid's ContentSizeFitter must settle before its parent reads its height.
        // Otherwise the background keeps the previous height until the bag is reopened.
        var grid = PassiveItemBlockPanel.transform as RectTransform;
        if (grid == null) return;
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid);
        for (var parent = grid.parent as RectTransform; parent != null;
            parent = parent.parent as RectTransform)
        {
            if (parent.GetComponent<Canvas>() != null) break;
            if (parent.GetComponent<LayoutGroup>() != null || parent.GetComponent<ContentSizeFitter>() != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
    }
}
