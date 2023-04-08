using StringLiterals;
using System.Collections;
using UnityEngine;

public class BLBook : Weapon
{
    private Vector2 _initPos;
    private Transform[] _books;
    protected override void Awake()
    {
        base.Awake();
        _initPos = transform.position;
    }
    private void Update()
    {
        Move();
    }
    protected override void Move()
    {
        transform.Rotate(Vector3.back, weaponStat.ProjectileSpeed);
        for (int i = 0; i < _books.Length; ++i)
        {
            _books[i].transform.Rotate(Vector3.forward, weaponStat.ProjectileSpeed);
        }
    }

    private IEnumerator _attackSequenceCoroutine;
    public override IEnumerator AttackSequence()
    {
        while (true)
        {
            gameObject.SetActive(true);

            yield return AttackDurationTime;


            gameObject.SetActive(false);

            yield return AttackRemainTime;
        }
    }
    public override void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        GameObject newBook;
        _books = new Transform[weaponStat.ProjectileCount];
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            newBook = new GameObject(nameof(BLBook));
            newBook.transform.parent = transform;
            newBook.layer = LayerNum.WEAPON;

            BoxCollider2D collider = newBook.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size *= 20;

            newBook.AddComponent<SpriteRenderer>().sprite = weaponData.Display;

            _books[i] = newBook.GetComponent<Transform>();
        }

        switch (weaponStat.ProjectileCount)
        {
            case 3:
                _books[0].transform.position = _initPos+ Vector2.up * 50;
                _books[1].transform.position = _initPos+ Vector2.left * 50 * Mathf.Sqrt(3) / 2 + Vector2.down * 50 / 2;
                _books[2].transform.position = _initPos+ Vector2.right * 50 * Mathf.Sqrt(3) / 2 + Vector2.down * 50 / 2;
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }

        this.weaponStat = weaponStat;
        AttackDurationTime = new WaitForSeconds(this.weaponStat.AttackDurationTime);
        AttackRemainTime = new WaitForSeconds(this.weaponStat.BaseAttackSequenceTime - this.weaponStat.AttackDurationTime);
        transform.localScale *= weaponStat.Size;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.ENEMY))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SetDamage(enemy);
        }
    }

}