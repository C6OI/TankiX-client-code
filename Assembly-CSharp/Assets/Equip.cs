using UnityEngine;

namespace Assets {
    public class Equip : MonoBehaviour {
        static Equip equipped;

        public void Do() {
            Animator component = GetComponent<Animator>();

            if (!component.GetBool("equipped") && equipped != null && equipped != this) {
                equipped.GetComponent<Animator>().SetBool("equipped", false);
            }

            component.SetBool("equipped", !component.GetBool("equipped"));

            if (component.GetBool("equipped")) {
                equipped = this;
            }
        }

        public void Claim() {
            Animator component = GetComponent<Animator>();
            component.SetTrigger("claim");
        }

        public void Cancel() {
            Animator component = GetComponent<Animator>();
            component.SetBool("disassembled", false);
        }

        public void Dis() {
            Animator component = GetComponent<Animator>();
            component.SetBool("disassembled", true);
        }

        public void OnClaim() {
            Destroy(gameObject);
        }
    }
}