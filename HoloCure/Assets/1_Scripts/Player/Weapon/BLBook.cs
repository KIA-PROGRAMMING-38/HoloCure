using System.Collections;
using UnityEngine;

public class BLBook : Weapon
{
    private Transform[] _books;
    private Vector2[] _booksPos;
    private BoxCollider2D _collider;
    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
    }
    protected override void Operate()
    {
        transform.Rotate(Vector3.back, weaponStat.ProjectileSpeed);
        for (int i = 0; i < _books.Length; ++i)
        {
            _books[i].transform.Rotate(Vector3.forward, weaponStat.ProjectileSpeed);
        }
    }
    public override IEnumerator AttackSequence()
    {
        while (true)
        {
            gameObject.SetActive(true);

            for (int i = 0; i < _books.Length; ++i)
            {
                _books[i].transform.position = (Vector2)transform.position + _booksPos[i];
            }

            yield return attackDurationTime;

            gameObject.SetActive(false);

            yield return attackRemainTime;
        }
    }
    public override void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(weaponData, weaponStat);

        _books = new Transform[weaponStat.ProjectileCount];
        _booksPos = new Vector2[weaponStat.ProjectileCount];

        GameObject newBook;
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            newBook = new GameObject(nameof(BLBook));
            newBook.transform.parent = transform;
            newBook.layer = LayerNum.WEAPON;

            BoxCollider2D collider = newBook.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size *= _collider.size;

            newBook.AddComponent<SpriteRenderer>().sprite = weaponData.Display;

            float angle = (i * 360 / weaponStat.ProjectileCount) * Mathf.Deg2Rad;
            _booksPos[i] = (Vector2.right * Mathf.Cos(angle) + Vector2.up * Mathf.Sin(angle)) * 50;
            newBook.transform.position = (Vector2)transform.position + _booksPos[i];

            _books[i] = newBook.GetComponent<Transform>();
        }
    }
}