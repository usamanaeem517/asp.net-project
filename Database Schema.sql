create database rave
use rave
drop database rave
use reave
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---CREATING TABLES-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
create  table all_accounts(
ID int not null identity (1,1) unique, 
Email varchar(50) not null, 
[Password] varchar(20) not null,
acc_type int
primary key(ID, Email)
check (acc_type between 1 and 3 )
)
--------------------------------------------------------------------------------------------
create table Singers (
ID int not null identity(1,1) unique, 
Name varchar(20) not null,
acc_id int not null,
primary key(id),
foreign key ([acc_id]) references all_accounts(id)
)

---------------------------------------------------------------------------------------------

create table Songs (
ID int not null identity (1,1) unique, 
Name varchar(20) not null, 
SingerID int not null, 
Album_Title varchar(20),
Duration time not null, 
Link_Video varchar(2083), 
Release_Date date not null, 
Genre varchar(10), 
Rating float
primary key(ID)
foreign key(SingerID) references Singers(ID)
)


--------------------------------------------------------------------------------------------
create table [Users] (
ID int not null identity (1,1) unique,
[name] varchar(20) not null ,
acc_id int not null
primary key(id)
foreign key([acc_id]) references all_accounts(id)
)
--------------------------------------------------------------------------------------------
create table Concert_Managers (
ID int not null identity (1,1) unique, 
Name varchar(20) not null, 
acc_id int not null
primary key(id)
foreign key([acc_id]) references all_accounts(id)
)

--------------------------------------------------------------------------------------------

create table Playlist (
userID int, 
SongID int 
primary key (userid,songid)
foreign key(songID) references Songs(ID)
)
ALTER TABLE Playlist
ADD CONSTRAINT fk_playlist_user
foreign key(userID) references [users](ID)
----------------------------------------------------------------------------------------------------
create table Concerts (
ID int not null identity (1,1) unique,
Name varchar(20) not null, 
Concert_ManagerID int not null, 
[Date] date not null, 
[Time] time not null, 
Venue varchar(20) not null, 
Price int not null,
location varchar(2083),
seatsleft int not null ,
seatstaken int 
check (seatstaken >=0)
primary key (ID)
foreign key (Concert_ManagerID) references Concert_Managers(ID)
)

-------------------------------------------------------------------------------------------------------
create table Concert_singers (
concert_ID int not null, 
Concert_SingerID int not null
primary key (Concert_ID,concert_singerid)
foreign key (Concert_ID) references Concerts(ID)
)
ALTER TABLE Concert_singers
ADD CONSTRAINT FK_concertsinger
foreign key(Concert_SingerID) references Singers(ID)


-------------------------------------------------------------------------------------------------------
create table Trending (
SongID int not null,
rating float
primary key(songid)
foreign key (SongID) references Songs(ID)
)
-------------------------------------------------------------------------------------------------------
create table Concert_attendees (
concert_ID int, 
userID int not null
primary key (Concert_ID,userid)
foreign key (Concert_ID) references Concerts(ID)
)
ALTER TABLE Concert_attendees
ADD CONSTRAINT FK_attendees
foreign key(userID) references [users](ID)

-----------------------------------------------------
create table ratings(
ID int identity(1,1),
songID int, userID int, rating float
primary key (ID)
foreign key (songID) references Songs(id),
foreign key (userID) references [users](ID),
check( rating between 1.0 and 5.0)
)

-- STORED PROCEDURES / VIEWS
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-- TO GET ACCOUNT ID, THIS ACTS AS A HELPER FUNCTION
Create procedure getaccid @email varchar(50),@acc_type int,@accid int output
As
Begin
	set @accid=(select id
	from all_accounts
	where Email=@email and acc_type=@acc_type)
	

End



------------------------------------------------------------------------------------------------------------------------------------------------------------
-- SIGN UP
create procedure signup @email varchar(50),@password varchar(20),@name varchar(20),@acc_type varchar(10)
as
Begin
	if (@email is not null)
	begin
	 if(@password is not null)
	 begin
	  if (@acc_type='User')
	  begin
		if not exists(select * from all_accounts where @email=email and acc_type=1)
		begin
		insert into all_accounts values (@email,@password,1)
		declare @a_id int
		execute getaccid @email,1,@a_id output
		insert into users values (@name,@a_id)
		end
		else
		RAISERROR('User Account is already present!',16,1) 
	  end

	  else if (@acc_type='Singer')
	  begin
		if not exists(select * from all_accounts where @email=email and acc_type=2)
		begin
		insert into all_accounts values (@email,@password,2)
		declare @a_id2 int
		execute getaccid @email,2,@a_id2 output
		insert into singers values (@name,@a_id2)
		end
		else
		RAISERROR('Singer Account is already present!',16,1)
	  end

	  else if (@acc_type='Manager')
	  begin
		if not exists(select * from all_accounts where @email=email and acc_type=3)
		begin
		insert into all_accounts values (@email,@password,3)
		declare @a_id3 int
		execute getaccid @email,3,@a_id3 output
		insert into concert_managers values (@name,@a_id3)
		end
		else
		RAISERROR('Manager Account is already present!',16,1)
	  end

	  else
	  RAISERROR('Account Type is incorrect',16,1)

	 end

	 else
	 RAISERROR('Password is incorrect',16,1)

	end

	else
	RAISERROR('Email is incorrect!',16,1)
End




------------------------------------------------------------------------------------------------------------------------------------------------------------
-- SIGN IN
create procedure signin @email varchar(50),@password varchar(20),@acc_type int
as
Begin
	if (@email is not null)
	begin
	 if(@password is not null)
	 begin
	    if not exists(select * from all_accounts where @email=email  )
		RAISERROR('User does not exist',16,1)
		else
			begin
			if not exists(select * from all_accounts where @email=email and acc_type=@acc_type )
		    RAISERROR('Account Type does not exist',16,1)
			else
				begin
				if not exists(select * from all_accounts where @email=email and acc_type=@acc_type and [password]=@password )
				RAISERROR('Password is incorrect',16,1)
				end
			end
	  end
	 
	 else
	 RAISERROR('Invalid password',16,1)

	end

	else
	RAISERROR('Invalid username',16,1)
End



------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ADD SONGS (USED BY SINGER)
create procedure addingsong @name varchar(20) ,@singerID int ,@Album_Title varchar(20),@Duration time , @Link_Video varchar(2083),@Release_Date date ,@Genre varchar(10)
as
Begin
if(@name is not null)
begin 
	if(@singerid is not null and  exists (select * from singers where @singerid=id))
	begin 
		if(not  exists (select * from singers where @singerid=id and name=@name))
		begin 
			if(@Release_Date is not null)
			begin 
			 insert into songs values(@name ,@singerID,@Album_Title ,@Duration, @Link_Video,@Release_Date,@Genre , NULL)

			end
			else
			RAISERROR('Invalid file',16,1)
		end
		else
		RAISERROR('song already exists',16,1)
	end
	else
	RAISERROR('Invalid singer id',16,1)
end
else
RAISERROR('Invalid name',16,1)
end


------------------------------------------------------------------------------------------------------------------------------------------------------------
-- FETCH SINGER DETAILS, ACTS AS A HELPER FUNCTION
create procedure fetch_singer_details
@email varchar(50)
as
begin
	select * from Singers join all_accounts on Singers.acc_id = all_accounts.ID where all_accounts.Email = @email
end


------------------------------------------------------------------------------------------------------------------------------------------------------------
-- FETCH USER DETAILS, ACTS AS A HELPER FUNCTION
create procedure fetch_user_details
@email varchar(50)
as
begin
	select * from Users join all_accounts on Users.acc_id = all_accounts.ID where all_accounts.Email = @email
end

------------------------------------------------------------------------------------------------------------------------------------------------------------
-- FETCH MANAGER DETAILS, ACTS AS A HELPER FUNCTION
create procedure fetch_manager_details
@email varchar(50)
as
begin
	select * from Concert_Managers join all_accounts on Concert_Managers.acc_id = all_accounts.ID where all_accounts.Email = @email
end


------------------------------------------------------------------------------------------------------------------------------------------------------------
-- GET SONG ID, GETS SONG ID SO THAT .MP3 FILE CAN BE RENAMED TO IT
create procedure get_songID
@sID int, @song_name varchar(20), @album_title varchar(20)
as
begin
	select * from songs where SingerID=@sID AND name=@song_name AND album_title=@album_title
end


------------------------------------------------------------------------------------------------------------------------------------------------------------
-- FETCH SONGS BY A SINGER ONLY
create procedure fetch_songs
@sID int
as
begin
	select Songs.ID, Songs.name, Songs.album_title, singers.name as sname, songs.duration, songs.genre,songs.Release_Date ,all_ratings.avg_rating as rating, songs.Link_Video from Songs join Singers on Songs.SingerID = Singers.ID  left outer join all_ratings on Songs.ID=all_ratings.SongID where Songs.SingerID=@sID
end

------------------------------------------------------------------------------------------------------------------------------------------------------------
-- FETCH ALL SONGS
create procedure fetch_all_songs
as
begin
	select Songs.ID, Songs.name, Songs.album_title, singers.name as sname, songs.duration, songs.genre,songs.Release_Date ,all_ratings.avg_rating as rating, songs.Link_Video from Songs join Singers on Songs.SingerID = Singers.ID  left outer join all_ratings on Songs.ID=all_ratings.SongID order by songs.name
end

------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ADD TO PLAYLIST
create procedure add_to_playlist
@userID int, @songID int
as
begin
if not exists (select * from playlist where userID=@userID and songID=@songID)
begin
insert into Playlist values(@userID, @songID)
end
end

------------------------------------------------------------------------------------------------------------------------------------------------------------
--GET SONGS FROM PLAYLIST
create procedure fetch_to_playlist
@userID int
as
begin
select Songs.ID, Songs.name, Songs.album_title, singers.name as sname, songs.duration, songs.genre,songs.Release_Date ,all_ratings.avg_rating as rating, songs.Link_Video  from Playlist join songs on Playlist.SongID=Songs.ID join singers on songs.singerid = singers.id  left outer join all_ratings on Songs.ID=all_ratings.SongID where userID=@userID
end


------------------------------------------------------------------------------------------------------------------------------------------------------------
-- VIEW THAT SHOWS AVERAGE RATINGS
create view all_ratings
as
select songID,avg(rating) avg_rating from ratings group by songID


------------------------------------------------------------------------------------------------------------------------------------------------------------
-- RATE SONGS
create procedure rate_song
@songID int, @userID int, @rating float
as
begin
if not exists (select * from ratings where userID=@userID and songID=@songID)
begin
insert into ratings values (@songID, @userID, @rating)
end
else
begin
update ratings set rating=@rating where userID=@userID AND songID=@songID
end
end

------------------------------------------------------------------------------------------------------------------------------------------------------------
-- GET TRENDING SONGS
create procedure fetch_trending
as
begin
select top(5) Songs.ID, Songs.name, Songs.album_title, singers.name as sname, songs.duration, songs.genre,songs.Release_Date ,all_ratings.avg_rating as rating, songs.Link_Video  from all_ratings join songs on Songs.ID=all_ratings.SongID join singers on songs.singerid = singers.id order by all_ratings.avg_rating desc
end


-- get concerts upcoming and done
------------------------------------------------------------------------------------------------------------------------------------------------------------
create view upcoming_concerts
as
select * from concerts where (cast (getdate()  as date) < date) OR ((Cast (GETDATE() as date) = date) and (cast(getdate() as time) < time))



create view finished_concerts
as
select * from concerts where (cast (getdate()  as date) > date) OR ((Cast (GETDATE() as date) = date) and (cast(getdate() as time) >= time))
------------------------------------------------------------------------------------------------------------------------------------------------------------

create procedure concert_upcoming_singer
@sID int
as
begin
select UC.Name, UC.Date, UC.Time, UC.Venue, UC.Price, UC.Seatsleft , UC.seatstaken, UC.location  from upcoming_concerts as UC  join Concert_singers C on C.concert_ID = UC.ID join singers S on C.Concert_SingerID=S.ID where S.ID=@sID
end

create procedure concert_finished_singer
@sID int
as
begin
select FC.Name, FC.Date, FC.Time, FC.Venue, FC.Price, FC.Seatsleft,FC.seatstaken,FC.location from finished_concerts as FC join Concert_singers C on C.concert_ID = FC.ID join singers S on C.Concert_SingerID=S.ID where S.ID=@sID
end

create procedure concert_upcoming_manager
@mID int
as
begin
select UC.Name, UC.Date, UC.Time, UC.Venue, UC.Price, UC.Seatsleft , UC.seatstaken, UC.location from upcoming_concerts UC where UC.concert_managerid=@mID
end

create procedure concert_finished_manager
@mID int
as
begin
select FC.Name, FC.Date, FC.Time, FC.Venue, FC.Price, FC.Seatsleft,FC.seatstaken, FC.location from finished_concerts as FC where FC.concert_managerID=@mID
end


------------------------------------------------------------------------------------------------------------------------------------------------------------
---Add Concert
create procedure add_concert
@name varchar(20), @cID int, @date date, @time time, @venue varchar(20), @s_email varchar(40), @price int, @seats int, @location varchar(2083)
as
begin
if exists (select singers.id from singers join all_accounts on singers.acc_id=all_accounts.id where all_accounts.Email=@s_email)
begin
declare @sid int
set @sid = (select singers.id from singers join all_accounts on singers.acc_id=all_accounts.id where all_accounts.Email=@s_email)
declare @stak int
set @stak=0
insert into Concerts values (@name, @cID, @date, @time, @venue, @price,  @location,@seats,@stak)
declare @concertID int
set @concertID = (select top (1) ID from Concerts order by id desc)
insert into Concert_singers values (@concertID, @sid)
end
end
------------------------------------------------------------------------------------------------------------------------------------------------------------

create procedure get_all_up_concerts
as
begin
select UC.ID, UC.Name, UC.Date, UC.Time, UC.Venue, UC.Price, UC.Seatsleft,Uc.seatstaken, UC.location from upcoming_concerts UC
end

------------------------------------------------------------------------------------------------------------------------------------------------------------
--Attend Concert
create procedure add_to_concert_attend
@userID int, @concertID int
as
begin
if not exists(select CA.concert_ID, CA.userID from Concert_attendees as CA where @userID = CA.userID AND @concertID = CA.concert_ID)
begin
declare @avl int
set @avl = (select seatsleft from Concerts where Concerts.ID=@concertID)
if(@avl > 0)
begin
insert into concert_attendees values (@concertID, @userID)
update concerts set seatsleft = (@avl - 1) where ID = @concertID
declare @tak int
set @tak = (select seatstaken from Concerts where Concerts.ID=@concertID)
update concerts set seatstaken = (@tak + 1) where ID = @concertID
end
else
begin
RAISERROR('No seats are available',16,1)
end
end
else
begin
RAISERROR('Concert Already added',16,1)
end
end


------------------------------------------------------------------------------------------------------------------------------------------------------------
create procedure fetch_attending_concerts
@userID int
as
begin
select UC.ID, UC.Name, UC.Date, UC.Time, UC.Venue, UC.Price, Uc.seatsleft,uc.seatstaken, UC.location from concerts UC join Concert_attendees C_A on C_A.concert_ID=UC.ID where C_A.userID=@userID 
end


create procedure get_emails
as
begin
select email from all_accounts
end

