# FashionStoreManagement

**Ngắn gọn:** Ứng dụng quản lý cửa hàng thời trang viết bằng C# (WPF). Dự án này chứa UI (WPF), model và cấu hình kết nối cơ sở dữ liệu.

---

## Mô tả

FashionStoreManagement là một dự án mẫu để quản lý sản phẩm, đơn hàng và người dùng cho cửa hàng thời trang. Dự án phù hợp để học, demo tính năng WPF + EF Core (nếu sử dụng) và làm nền tảng mở rộng.

## Tính năng chính

* Quản lý sản phẩm (danh sách, thêm/sửa/xóa)
* Xem giao diện đăng nhập
* Các trang hiển thị sản phẩm (ProductsView)
* Cấu trúc project sẵn sàng để mở rộng (Models, Views, ViewModels...)

> Ghi chú: Một số chức năng có thể đang trong trạng thái phát triển — kiểm tra code để biết chi tiết.

## Yêu cầu

* Windows (WPF)
* Visual Studio 2019/2022 (recommended) hoặc .NET SDK tương thích với file `.csproj`
* SQL Server (hoặc DB tương thích) nếu dự án kết nối DB

## Cài đặt & chạy

1. Clone repository:

```bash
git clone https://github.com/dtranh2005/FashionStoreManagement.git
cd FashionStoreManagement
```

2. Mở file `FashionStoreManagement.sln` bằng Visual Studio.

3. Cập nhật chuỗi kết nối trong `appsettings.json` (nếu có) hoặc trong phần cấu hình kết nối DB trong code. Ví dụ placeholder:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=FashionStoreDB;Trusted_Connection=True;"
  }
}
```

4. Restore và build solution trong Visual Studio.

5. Chạy project (Start Debugging) hoặc build & run.

## Cấu trúc chính của repo

* `Models/` - các lớp dữ liệu (Product, Order, …)
* `*.xaml`, `*.xaml.cs` - Views (WPF)
* `appsettings.json` - cấu hình (chuỗi kết nối, settings)
* `FashionStoreManagement.sln` - solution

## Lưu ý khi phát triển

* Kiểm tra các phần tương tác với DB (OrderItems, OrderDetails) nếu gặp lỗi hiển thị dữ liệu.
* Nếu UI không hiện đủ items (ví dụ chỉ hiện 2/3 item), kiểm tra binding trong XAML và nguồn dữ liệu (ItemsSource, Collection được load từ DB).

## Muốn thêm ảnh màn hình?

Bạn có thể thêm thư mục `docs/screenshots` hoặc `assets` và gọi chúng trong README bằng markdown để minh họa.

## Góp ý / đóng góp

* Mở issue hoặc pull request để đóng góp tính năng hoặc sửa lỗi.
* Nếu muốn, tôi có thể giúp viết hướng dẫn triển khai DB (script tạo bảng) dựa trên models hiện có.

## License

Mặc định không có license — nếu bạn muốn, hãy thêm file `LICENSE` (MIT/GPL/...) phù hợp.

---

Nếu muốn mình sửa đổi nội dung (tiếng Anh, thêm badge, hướng dẫn chi tiết cho DB, hoặc file `.sql` để tạo schema), nói ngay mình sẽ cập nhật.
