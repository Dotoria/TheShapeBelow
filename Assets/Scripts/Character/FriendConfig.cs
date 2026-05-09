using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "FriendConfig", menuName = "Configs/Friend Config")]
    public class FriendConfig : CharacterConfigBase<Friend, Friend.EFriendType>
    {
        protected override void ApplyConfig(
            Friend friend,
            CharacterData<Friend, Friend.EFriendType> config
        )
        {
            friend.ApplyConfig(config);
        }
    }
}