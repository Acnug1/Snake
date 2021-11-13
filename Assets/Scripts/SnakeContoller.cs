using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeContoller : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEat; // создаем систему событий в инспекторе
    [SerializeField] private List<Transform> _tails; // список костей хвоста
    [SerializeField] [Range(0, 3)] private float _bonesDistance; // дистанция между элементами хвоста
    [SerializeField] private GameObject _bonePrefab; // шаблон элемента нашего хвоста
    [SerializeField] [Range(0, 4)] private float _speed; // Range(0, 4) - генерирует слайдер в инспекторе для задания значений скорости от 0 до 4
    [SerializeField] private Transform _parent;

    private Transform _transform; // закэшируем компонент _transform, с помощью которого мы будем двигать нашу змейку
    Vector3 previousPosition;

    public event UnityAction ScoreChanged;
    public event UnityAction Died;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        MoveSnake(_transform.position + _transform.forward * _speed * Time.deltaTime); // двигаем змейку вперед каждый кадр

        float angle = Input.GetAxis("Horizontal") * 150 * Time.deltaTime; // Input.GetAxis("Horizontal") - возвращает значения от -1 до 1 (включая промежуточные значения от 0), в зависимости от того, какую клавишу нажал игрок - влево или вправо

        _transform.Rotate(0, angle, 0); // поворачиваем змейку по оси Y на определенный угол (преобразуем вектор в угол поворота Quaternion.Euler)
    }

    private void MoveSnake(Vector3 newPosition)
    {
        float sqrDistance = _bonesDistance * _bonesDistance; // высчитаем квадрат нашей дистанции (для сравнения значений)
        previousPosition = _transform.position; // определим позицию предыдущей кости, в данном случае это позиция нашей головы

        foreach (var bone in _tails)
        {
            if ((bone.position - previousPosition).sqrMagnitude > sqrDistance) // если квадрат дистанции между костьми больше, чем заданное значение 
                                                                               // (делаем сравнение квадратов, так как извлечение корня довольно затратная операция, таким образом мы можем выиграть в производительности)
            {
                var temp = bone.position; // запоминаем позицию текущей кости
                bone.position = previousPosition; // двигаем кость на позицию предыдущей кости
                bone.rotation = _transform.rotation; // поворачиваем кость на угол поворота головы
                previousPosition = temp; // запоминаем предыдущую позицию кости, которую мы только что передвинули
            }
            else
            {
                break; // прерываем выполнение foreach, если дистанция между костьми не изменилась
            }
        }

        _transform.position = newPosition;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Food food)) // если мы столкнулись с объектом имеющим компонент Food
        {
            Destroy(collision.gameObject); // удаляем объект, с которым мы столкнулись

            var newBone = Instantiate(_bonePrefab, previousPosition, _transform.rotation, _parent); // создаем новую кость из префаба
            _tails.Add(newBone.transform); // добавляем в массив transform только что созданной кости

            OnEat?.Invoke();
            ScoreChanged?.Invoke();
        }
          
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) &&
            (hit.collider.gameObject.TryGetComponent(out Wall wall) || 
            hit.collider.gameObject.TryGetComponent(out Bone bone)))
        {
            //   Debug.DrawRay(transform.position, transform.forward, Color.red);
            Destroy(gameObject); // если змейка столкнулась с препятствием она умирает
            Time.timeScale = 0;

            Died?.Invoke();
        }
    }
}
