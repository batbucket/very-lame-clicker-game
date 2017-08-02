using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    private const string COUNT_STORE_KEY = "count";

    private static readonly int[] TROPHY_IDS = new int[]
    {
      80821, // 1
      80822, // 10
      80825,

      80823,
      80824,
      80826,

      80827,
      80828,
      80829,

      80830,
      80831,
      80832,
      80833, // 4B
      80834  // uint.max
    };

    private uint count;
    private float programRunDuration;

    [SerializeField]
    private Text bigCountDisplay;
    [SerializeField]
    private Text smallCountDisplay;

    [SerializeField]
    private Text userNotSignedInDisplay;

    private ulong Count {
        set {
            if (value < uint.MaxValue) {
                this.bigCountDisplay.text = value.KiloFormat();
                this.smallCountDisplay.text = value.ToString();
            } else {
                this.bigCountDisplay.text = "You broke the game!";
                this.smallCountDisplay.text = string.Format("{0} is the maximum value!", value.ToString());
            }
        }
    }

    private bool IsUserSignedIn {
        get {
            return GameJolt.API.Manager.Instance.CurrentUser != null;
        }
    }

    public void IncrementCount() {
        if (count < uint.MaxValue) {
            this.count++;
        }
        AttemptToRewardTrophy();
        AttemptToUpdateLeaderboards();
        SaveProgress();
    }

    public void ShowLeaderboards() {
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
    }

    public void ShowAchievements() {
        GameJolt.UI.Manager.Instance.ShowTrophies();
    }

    private void Start() {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart() {
        yield return new WaitForSeconds(1);
        GameJolt.API.DataStore.Get(
            COUNT_STORE_KEY,
            false,
            retrieved => {
                Debug.Log("Attempting to load " + retrieved);
                uint loadedCount;
                uint.TryParse(retrieved, out loadedCount);
                Debug.Log("Loaded " + loadedCount);
                this.count = loadedCount;
            }
            );
    }

    private void Update() {
        Count = this.count;
        userNotSignedInDisplay.gameObject.SetActive(!IsUserSignedIn);
    }

    private void SaveProgress() {
        GameJolt.API.DataStore.Get(
        COUNT_STORE_KEY,
        false,
        retrieved => {
            uint retrievedCount;
            uint.TryParse(retrieved, out retrievedCount);
            Debug.Log("Retrieved " + retrievedCount);
            if (retrievedCount < count) {
                GameJolt.API.DataStore.Set(
                    COUNT_STORE_KEY,
                    count.ToString(),
                    false,
                    b => Debug.Log(string.Format("Save {0} attempt: {1}", count, b))
                    );
            }
        }
        );
    }

    private void AttemptToUpdateLeaderboards() {
        GameJolt.API.Scores.Add(
            count.ToString(),
            string.Format("{0} clicks.", count),
            string.Empty,
            0,
            string.Empty
            );
    }

    private void AttemptToRewardTrophy() {
        int awardedTrophyID = GetTrophyID(count);
        if (awardedTrophyID > 0) {
            GameJolt.API.Trophies.Unlock(awardedTrophyID);
        }
    }

    private int GetTrophyID(uint count) {
        switch (count) {
            case 1:
                return TROPHY_IDS[0];
            case 10:
                return TROPHY_IDS[1];
            case 100:
                return TROPHY_IDS[2];
            case 1000:
                return TROPHY_IDS[3];
            case 10000:
                return TROPHY_IDS[4];
            case 100000:
                return TROPHY_IDS[5];
            case 1000000:
                return TROPHY_IDS[6];
            case 10000000:
                return TROPHY_IDS[7];
            case 100000000:
                return TROPHY_IDS[8];
            case 1000000000:
                return TROPHY_IDS[9];
            case 2000000000:
                return TROPHY_IDS[10];
            case 3000000000:
                return TROPHY_IDS[11];
            case 4000000000:
                return TROPHY_IDS[12];
            case uint.MaxValue:
                return TROPHY_IDS[13];
        }
        return -1;
    }
}
