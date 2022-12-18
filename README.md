Приложение по производственной практике
=====================
### [Ссылка на дизайн](https://www.figma.com/file/PyTJx6TueDF8T5ys2i3Gsu/Строительная-компания?node-id=0%3A1&t=NQNMBSHlppQanr13-0)
Task лист
------------------
1. [x] Дизайн
   1. [x] Диалоговое окно выбора
   2. [x] Панель навигации
   3. [x] Окно редактирования/создания дома
   4. [x] Страница домов
2. [ ] Разработка
   1. [x] Панель навигации
      1. [x] Вывод фотографии и ФИО авторизовавшегося пользователя
      2. [x] Вывод кнопок навигации по страницам
      3. [x] Добавить кнопку выхода из аккаунта ползователя
   2. [x] Стили для элементов управления
   3. [x] Страница домов
      1. [x] Поиск
      2. [x] Сортировка (По названию, по цене, по площади дома). Помимо выбора поля сортировки, должна быть кнопка выбора направления сортировки
      3. [x] Вывод домов плитками
      4. [x] Добавление домов
      5. [x] Удаление домов
      6. [x] Пометка домов
   4. [x] Страница материалов
      1. [x] Поиск
      2. [x] Сортировка (По названию, по цене, по количеству). Помимо выбора поля сортировки, должна быть кнопка выбора направления сортировки
      3. [x] Вывод материалов таблицей
      4. [x] Добавление материалов
      5. [x] Удаление материалов
      6. [x] Пометка материалов
   5. [x] Страница поставщиков
      1. [x] Поиск
      2. [x] Сортировка (По названию, ID). Помимо выбора поля сортировки, должна быть кнопка выбора направления сортировки
      3. [x] Вывод поставщиков таблицей
      4. [x] Добавление поставщиков
      5. [x] Удаление поставщиков
      6. [x] Пометка поставщиков
   6. [ ] Логика работы с заказами
   7. [ ] Окно авторизации
      1. [ ] Страница входа
         1. [ ] Валидация полей ввода
         2. [ ] Вывод ошибок
      3. [ ] Страница регистрации клиента
         1. [ ] Валидация полей ввода
         2. [ ] Вывод ошибок
3. [x] База данных
   1. [x] Удалить таблицы перечислений и добавить их кодом

Функционал для ролей
=================================
Клиент
--------------------------
1. Просмотреть дома
2. Создать заказ
3. Посмотреть свои заказы
4. Оплатить заказ

Админ
-----------------
1. Просматривать и редактировать дома
2. Просматривать заказы
3. Просматривать поставки
4. Просматривать и редактировать поставщиков
5. Просматривать и редактировать материалы

Сотрудник
-------------------
### Рабочий
Не имеет доступа к приложению
### Кладовщик
1. Просматривать и редактировать материалы
2. Создавать документ поставки
3. Просматривать список домов
### Менеджер
1. Просматривать свои заказы
2. Создавать заказы
3. Просматривать список домов
### Кадровик
1. Просматривать и редактировать сотрудников
