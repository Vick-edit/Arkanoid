# Пример реализации игры арканоид

### Челендж
Создать игру Арканоид без использования встроенной физики, конечно полностью это не удалось, было бы слишком муторно, в проекте использованы Physics2D.Raycast, но не использовано не единого RigidBody
Суть проекта в попытке объединить две концепции КОП и MVC и проверка гипотезы о полезности пула эффектов EffectPool и эвент системы на базе диспетчера Dispatcher.

### Описание реализации
На GameObject навешиваются компоненты, у компонента основное поле - это класс, который больше похож на DTO, то есть это набор каких-то полей без логики обработки - это модель в концепции MVC, эта модель передаётся в фабрику, которая заменяет DI, чтобы не сильно замедлять работу проекта рефлекшеном. Фабрика создаёт реализацию интерфейса контроллера, в которую прокидывается компонента, который выполнять роль вида (view) и DTO представление модели. Всю логику поведения модели реализует контроллер. Таким образом, мы можем на лету в эдиторе менять параметры и видеть их изменения контроллером, т.к. класс модели сериализуем, мы можем легко заменить или расширить проект добавив новую реализацию контроллера, если она должна использоваться только в специфических ситуациях мы добавляем новый метод в фабрику, если этот контроллер должен заменить дефолтное поведение - ещё проще, код меняется только в одном месте.
