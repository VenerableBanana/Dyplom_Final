using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    class PlayerController2D : MonoBehaviour
    {
        Rigidbody2D rigidbody;
        Vector2 velocity;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * 20;
        }

        void FixedUpdate()
        {
            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        }
	}
}
