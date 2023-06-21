# Hướng dẫn Lập trình Enemy
Chào mừng bạn đến với hướng dẫn lập trình Enemy! Trong tài liệu này, chúng ta sẽ tìm hiểu cách tạo và quản lý các kẻ thù trong game  bằng ngôn ngữ lập trình C#.

## Giới thiệu
Trong game, các kẻ thù là những nhân vật phản diện mà Player cần đối đầu. Các kẻ thù có thể di chuyển, tấn công hoặc tương tác với Player và môi trường xung quanh. Chúng là một phần quan trọng để tăng tính thách thức và độ phong phú của trò chơi.

## Các bước cơ bản
Dưới đây là các bước cơ bản để lập trình một kẻ thù trong game Player:

1. Tạo lớp Enemy: Đầu tiên, tạo một lớp hoặc cấu trúc dữ liệu đại diện cho kẻ thù (Enemybase). Lớp này nên chứa các thuộc tính như vị trí, tốc độ di chuyển, trạng thái hiện tại và các thuộc tính khác cần thiết.

``` 
public class Enemy
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public string State { get; set; }

    public void Update()
    {
        // Cập nhật trạng thái và vị trí của kẻ thù
    }

    public void Render()
    {
        // Hiển thị kẻ thù lên màn hình
    }

    public void HandleCollision(GameObject otherObject)
    {
        // Xử lý va chạm với đối tượng khác
    }

    public void Move()
    {
        // Xử lý hành vi di chuyển của kẻ thù
    }

    public void Attack()
    {
        // Xử lý hành vi tấn công của kẻ thù
    }
} 
```

2. Quản lý vị trí: Sử dụng các phương thức và thuộc tính để cập nhật và quản lý vị trí của kẻ thù. Vị trí có thể được lưu dưới dạng tọa độ (x, y) hoặc thông qua hệ thống lưới trong game.

3. Xử lý va chạm: Đảm bảo rằng kẻ thù có thể phản ứng với va chạm với Player hoặc các đối tượng khác trong môi trường. Xử lý va chạm để xác định xem kẻ thù có bị tiêu diệt hay làm mất máu của Player hay không.

4. Di chuyển: Lập trình các hành vi di chuyển cho kẻ thù. Điều này có thể bao gồm việc di chuyển theo một đường cố định, đuổi theo Player, lặp lại các mẫu di chuyển, hoặc thậm chí có thể sử dụng thuật toán AI đơn giản để quyết định cách di chuyển.

5. Hành vi tấn công: Nếu kẻ thù của bạn có khả năng tấn công, lập trình các hành vi tấn công tương ứng. Điều này có thể bao gồm việc bắn đạn, tung lửa, hoặc tấn công trực tiếp Player.

Trạng thái và quản lý hành vi: Sử dụng các biến trạng thái để theo dõi trạng thái hiện tại của kẻ thù và quyết định hành vi tương ứng. Ví dụ: trạng thái "bình thường", "tấn công", "chết"...


## Cách tiếp cận tổ chức
Để tăng tính linh hoạt và dễ bảo trì của mã:

- Kế thừa: Tạo một lớp cơ sở Enemy và kế thừa từ đó để tạo các loại kẻ thù khác nhau như Goomba, Koopa Troopa, v.v. Điều này cho phép chúng chia sẻ các hành vi chung trong khi vẫn có thể có những hành vi riêng biệt.
```
public class Goomba : Enemy
{
    public Goomba(Vector2 position)
    {
        Position = position;
        // Các thuộc tính riêng của Goomba
    }

    public override void Update()
    {
        base.Update();
        // Cập nhật Goomba
    }

    public override void Render()
    {
        base.Render();
        // Hiển thị Goomba
    }

    public override void HandleCollision(GameObject otherObject)
    {
        base.HandleCollision(otherObject);
        // Xử lý va chạm của Goomba
    }

    public override void Move()
    {
        base.Move();
        // Xử lý di chuyển của Goomba
    }

    public override void Attack()
    {
        base.Attack();
        // Xử lý tấn công của Goomba
    }
}

```
- Giao diện: Định nghĩa một giao diện hoặc một tập hợp các phương thức chung mà tất cả các kẻ thù cần triển khai. Điều này đảm bảo rằng tất cả các kẻ thù phải có các phương thức quan trọng như update(), render(), v.v.

- Quản lý: Tạo một quản lý trung gian, chẳng hạn như EnemyManager, để quản lý tất cả các kẻ thù trong game. Điều này giúp theo dõi và điều khiển tất cả các kẻ thù từ một điểm tập trung.
```
public class EnemyManager
{
    private List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(Enemy enemy)
    {
        // Thêm kẻ thù vào danh sách quản lý
    }

    public void RemoveEnemy(Enemy enemy)
    {
        // Xóa kẻ thù khỏi danh sách quản lý
    }

    public void Update()
    {
        // Cập nhật tất cả kẻ thù
    }

    public void Render()
    {
        // Hiển thị tất cả kẻ thù
    }

    public void CheckCollision(GameObject otherObject)
    {
        // Kiểm tra va chạm với các kẻ thù
    }

    public void HandleInput(InputEvent input)
    {
        // Xử lý các sự kiện đầu vào cho kẻ thù
    }
}

```

## Kết luận
Lập trình kẻ thù trong game có thể thú vị và phức tạp. Bằng cách tạo lớp Enemy, quản lý vị trí, xử lý va chạm, lập trình hành vi di chuyển và tấn công, và sử dụng các phương pháp tổ chức, mình có thể tạo ra các kẻ thù độc đáo và thú vị cho trò chơi của mình.