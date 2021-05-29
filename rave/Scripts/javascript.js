function checkForEmptyFieldsSignup() {
    var userName = document.getElementById("Username").value;
	var password = document.getElementById("Password").value;
	var email = document.getElementById("Email").value;
	var usertype = document.getElementById("account-type").value;
	var len = password.length;

	if (userName === "" || password === "" || email ==="") {
		alert("Please Enter All Fields");
		return (false)
	}
	if(email.search("@") === -1){
		alert("Enter Valid Email Format!");
			return (false)
	}
	if (email.search(".") === -1) {
		alert("Invalid Email Address");
		return (false)
	}
	
	if (len < 8) {
		alert("Password length must be atleast 8 characters");
		return (false)
	}
	return (true)
};
