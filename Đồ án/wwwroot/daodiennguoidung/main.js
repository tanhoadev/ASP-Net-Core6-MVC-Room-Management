function validateSignupForm() {
    var newPassword = document.getElementById("newPassword").value;
    var confirmPassword = document.getElementById("confirmPassword").value;
    var Password = document.getElementById("Password").value;
    if (newPassword !== confirmPassword) {
        alert("Mật khẩu mới và nhập lại mật khẩu mới không khớp nhau. Vui lòng kiểm tra lại.");
        event.preventDefault();
    }
    if (newPassword == Password) {
        alert("Thay đổi mật khẩu thất bại. Vui lòng kiểm tra lại mật khẩu và đảm bảo mật khẩu mới không giống mật khẩu cũ");
        event.preventDefault();
    }
}