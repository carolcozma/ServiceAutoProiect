using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAutoProiect.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity.Data;
namespace ServiceAutoProiect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public CustomersController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        [Route("GetAllCustomers")]

        public string GetCustomers()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [ServiceAutoProiect].[dbo].[Customers]", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Customer> customersList = new List<Customer>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Customer customer = new Customer();
                    customer.Id = Convert.ToInt32(dt.Rows[i]["customer_id"].ToString());
                    customer.FirstName = dt.Rows[i]["first_name"].ToString();
                    customer.LastName = dt.Rows[i]["last_name"].ToString();
                    customer.Address = dt.Rows[i]["address"].ToString();
                    customer.Email = dt.Rows[i]["email"].ToString();
                    customer.Phone = dt.Rows[i]["phone"].ToString();
                    customersList.Add(customer);
                }
            }
            if (customersList.Count > 0)
            {
                return JsonConvert.SerializeObject(customersList);
            }
            else
            {
                response.Status = "100";
                response.Message = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("AddCustomer")]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Check if the customer already exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM [ServiceAuto].[dbo].[Customers] WHERE Email = @Email", con);
                checkCmd.Parameters.AddWithValue("@Email", customer.Email);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    return StatusCode(409, new { Status = "100", Message = "Customer already exists" });

                }

                // Insert the new customer
                SqlCommand cmd = new SqlCommand("INSERT INTO [ServiceAuto].[dbo].[Customers] (FirstName, LastName, Address, Email, Phone, Password) VALUES (@FirstName, @LastName, @Address, @Email, @Phone, @Password)", con);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Password", customer.Password);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { Status = "200", Message = "Customer added successfully" });
                }
                else
                {
                    return StatusCode(500, new { Status = "100", Message = "Failed to add customer" });
                }
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] Models.LoginRequest loginRequest)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Check if the customer exists and the password matches
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [ServiceAuto].[dbo].[Customers] WHERE Email = @Email AND Password = @Password", con);
                cmd.Parameters.AddWithValue("@Email", loginRequest.Email);
                cmd.Parameters.AddWithValue("@Password", loginRequest.Password);
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    return Ok(new { Status = "200", Message = "Login successful" });
                }
                else
                {
                    return Unauthorized(new { Status = "401", Message = "Invalid email or password" });
                }
            }
        }

        [HttpGet]
        [Route("GetCustomerByEmail")]
        public IActionResult GetCustomerByEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Retrieve customer information based on email
                SqlCommand cmd = new SqlCommand("SELECT * FROM [ServiceAuto].[dbo].[Customers] WHERE Email = @Email", con);
                cmd.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        Id = Convert.ToInt32(reader["CustomerID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Address = reader["Address"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Password = reader["Password"].ToString() // Note: Consider not returning the password for security reasons
                    };

                    return Ok(customer);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "Customer not found" });
                }
            }
        }

        [HttpPost]
        [Route("AddCar")]
        public IActionResult AddCar([FromBody] Car car)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Check if the customer exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM [ServiceAuto].[dbo].[Customers] WHERE CustomerID = @CustomerId", con);
                checkCmd.Parameters.AddWithValue("@CustomerId", car.CustomerID);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    return NotFound(new { Status = "404", Message = "Customer not found" });
                }

                // Insert the new car
                SqlCommand cmd = new SqlCommand("INSERT INTO [ServiceAuto].[dbo].[Cars] (CustomerId, Make, Model, Year, VIN, LicensePlate) VALUES (@CustomerId, @Make, @Model, @Year, @VIN,@LicensePlate)", con);
                cmd.Parameters.AddWithValue("@CustomerId", car.CustomerID);
                cmd.Parameters.AddWithValue("@Make", car.Make);
                cmd.Parameters.AddWithValue("@Model", car.Model);
                cmd.Parameters.AddWithValue("@Year", car.Year);
                cmd.Parameters.AddWithValue("@VIN", car.VIN);
                cmd.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { Status = "200", Message = "Car added successfully" });
                }
                else
                {
                    return StatusCode(500, new { Status = "100", Message = "Failed to add car" });
                }
            }
        }
        [HttpGet]
        [Route("GetCarsByCustomerId")]
        public IActionResult GetCarsByCustomerId(int customerId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Retrieve cars information based on customer ID
                SqlCommand cmd = new SqlCommand("SELECT * FROM [ServiceAuto].[dbo].[Cars] WHERE CustomerId = @CustomerId", con);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Car> carsList = new List<Car>();

                while (reader.Read())
                {
                    Car car = new Car
                    {
                        CarID = Convert.ToInt32(reader["CarId"]),
                        CustomerID = Convert.ToInt32(reader["CustomerId"]),
                        Make = reader["Make"].ToString(),
                        Model = reader["Model"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        VIN = reader["VIN"].ToString(),
                        LicensePlate = reader["LicensePlate"].ToString()
                    };

                    carsList.Add(car);
                }

                if (carsList.Count > 0)
                {
                    return Ok(carsList);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "No cars found for the given customer ID" });
                }
            }
        }

        [HttpGet]
        [Route("GetServicesByCarId")]
        public IActionResult GetServicesByCarId(int carId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Use the provided query to retrieve services information based on car ID
                string query = @"
            SELECT 
                s.ServiceID,
                s.Description,
                s.TotalCost,
                s.ServiceDate,
                s.Status,
                m.FirstName AS MechanicFirstName,
                m.LastName AS MechanicLastName
            FROM 
                Services s
            JOIN 
                Mechanics m ON s.MechanicID = m.MechanicID
            WHERE 
                s.CarID = @CarID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CarID", carId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<ServiceDetail> serviceDetailsList = new List<ServiceDetail>();

                while (reader.Read())
                {
                    ServiceDetail serviceDetail = new ServiceDetail
                    {
                        ServiceID = Convert.ToInt32(reader["ServiceID"]),
                        Description = reader["Description"].ToString(),
                        TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                        ServiceDate = Convert.ToDateTime(reader["ServiceDate"]),
                        Status = reader["Status"].ToString(),
                        MechanicFirstName = reader["MechanicFirstName"].ToString(),
                        MechanicLastName = reader["MechanicLastName"].ToString()
                    };

                    serviceDetailsList.Add(serviceDetail);
                }

                if (serviceDetailsList.Count > 0)
                {
                    return Ok(serviceDetailsList);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "No services found for the given car ID" });
                }
            }
        }
        [HttpGet]
        [Route("GetServiceTypes")]
        public IActionResult GetServiceTypes()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Retrieve service types information
                SqlCommand cmd = new SqlCommand("SELECT * FROM [ServiceAuto].[dbo].[ServiceTypes]", con);
                SqlDataReader reader = cmd.ExecuteReader();

                List<ServiceType> serviceTypesList = new List<ServiceType>();

                while (reader.Read())
                {
                    ServiceType serviceType = new ServiceType
                    {
                        ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]),
                        ServiceName = reader["ServiceName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"])
                    };

                    serviceTypesList.Add(serviceType);
                }

                if (serviceTypesList.Count > 0)
                {
                    return Ok(serviceTypesList);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "No service types found" });
                }
            }
        }
        [HttpPost]
        [Route("AddService")]
        public IActionResult AddService([FromBody] AddService addService)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Check if the car exists
                SqlCommand checkCarCmd = new SqlCommand("SELECT COUNT(*) FROM [ServiceAuto].[dbo].[Cars] WHERE CarId = @CarId", con);
                checkCarCmd.Parameters.AddWithValue("@CarId", addService.CarId);
                int carCount = (int)checkCarCmd.ExecuteScalar();

                if (carCount == 0)
                {
                    return NotFound(new { Status = "404", Message = "Car not found" });
                }

                // Retrieve the service type name
                SqlCommand getServiceTypeCmd = new SqlCommand("SELECT ServiceName FROM [ServiceAuto].[dbo].[ServiceTypes] WHERE ServiceTypeId = @ServiceTypeId", con);
                getServiceTypeCmd.Parameters.AddWithValue("@ServiceTypeId", addService.ServiceTypeId);
                string serviceTypeName = (string)getServiceTypeCmd.ExecuteScalar();

                if (string.IsNullOrEmpty(serviceTypeName))
                {
                    return NotFound(new { Status = "404", Message = "Service type not found" });
                }

                // Select a random mechanic
                SqlCommand getRandomMechanicCmd = new SqlCommand("SELECT TOP 1 MechanicId FROM [ServiceAuto].[dbo].[Mechanics] ORDER BY NEWID()", con);
                int mechanicId = (int)getRandomMechanicCmd.ExecuteScalar();

                // Insert the new service
                SqlCommand cmd = new SqlCommand("INSERT INTO [ServiceAuto].[dbo].[Services] (CarId, ServiceTypeId, MechanicId, Description, ServiceDate, TotalCost, Status) VALUES (@CarId, @ServiceTypeId, @MechanicId, @Description, @ServiceDate, @TotalCost, @Status)", con);
                cmd.Parameters.AddWithValue("@CarId", addService.CarId);
                cmd.Parameters.AddWithValue("@ServiceTypeId", addService.ServiceTypeId);
                cmd.Parameters.AddWithValue("@MechanicId", mechanicId);
                cmd.Parameters.AddWithValue("@Description", serviceTypeName);
                cmd.Parameters.AddWithValue("@ServiceDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@TotalCost", 0); // Assuming initial cost is 0, update as needed
                cmd.Parameters.AddWithValue("@Status", "Pending"); // Assuming initial status is "Pending", update as needed

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { Status = "200", Message = "Service added successfully" });
                }
                else
                {
                    return StatusCode(500, new { Status = "100", Message = "Failed to add service" });
                }
            }
        }
        [HttpGet]
        [Route("GetCarParts")]
        public IActionResult GetCarParts()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Retrieve car parts information
                SqlCommand cmd = new SqlCommand("SELECT PartId, PartName, Price, Filepaths, Quantity FROM [ServiceAuto].[dbo].[CarParts]", con);
                SqlDataReader reader = cmd.ExecuteReader();

                List<CarPart> carPartsList = new List<CarPart>();

                while (reader.Read())
                {
                    CarPart carPart = new CarPart
                    {
                        CarPartId = Convert.ToInt32(reader["PartId"]),
                        PartName = reader["PartName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        FilePath = reader["FilePaths"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };

                    carPartsList.Add(carPart);
                }

                if (carPartsList.Count > 0)
                {
                    return Ok(carPartsList);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "No car parts found" });
                }
            }
        }
        [HttpGet]
        [Route("GetServicePartsByServiceId")]
        public IActionResult GetServicePartsByServiceId(int serviceId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Use the provided query to retrieve service parts information based on service ID
                string query = @"
            SELECT 
                p.PartName,
                p.Price AS PartPrice,
                sp.Quantity AS PartQuantity
            FROM 
                ServiceParts sp
            JOIN 
                CarParts p ON sp.PartID = p.PartID
            WHERE 
                sp.ServiceID = @ServiceID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ServiceID", serviceId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<ServicePartDetail> servicePartDetailsList = new List<ServicePartDetail>();

                while (reader.Read())
                {
                    ServicePartDetail servicePartDetail = new ServicePartDetail
                    {
                        PartName = reader["PartName"].ToString(),
                        PartPrice = Convert.ToDecimal(reader["PartPrice"]),
                        PartQuantity = Convert.ToInt32(reader["PartQuantity"])
                    };

                    servicePartDetailsList.Add(servicePartDetail);
                }

                if (servicePartDetailsList.Count > 0)
                {
                    return Ok(servicePartDetailsList);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "No service parts found for the given service ID" });
                }
            }
        }

        [HttpDelete]
        [Route("DeleteCar")]
        public IActionResult DeleteCar(int carID)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    SqlCommand deleteServicePartsCmd = new SqlCommand("DELETE FROM ServiceParts WHERE ServiceID IN (SELECT ServiceID FROM Services WHERE CarID = @CarID)", con, transaction);
                    deleteServicePartsCmd.Parameters.AddWithValue("@CarID", carID);
                    deleteServicePartsCmd.ExecuteNonQuery();

                    SqlCommand deleteServicesCmd = new SqlCommand("DELETE FROM Services WHERE CarID = @CarID", con, transaction);
                    deleteServicesCmd.Parameters.AddWithValue("@CarID", carID);
                    deleteServicesCmd.ExecuteNonQuery();

                    SqlCommand deleteCarsCmd = new SqlCommand("DELETE FROM Cars WHERE CarID = @CarID", con, transaction);
                    deleteCarsCmd.Parameters.AddWithValue("@CarID", carID);
                    deleteCarsCmd.ExecuteNonQuery();

                    transaction.Commit();

                    return Ok(new { Status = "200", Message = "Car deleted successfully" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, new { Status = "100", Message = "Failed to delete car", Error = ex.Message });
                }
            }
        }
        [HttpDelete]
        [Route("DeleteService")]
        public IActionResult DeleteService(int serviceID)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Begin a transaction
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Delete from ServiceParts
                    SqlCommand deleteServicePartsCmd = new SqlCommand("DELETE FROM ServiceParts WHERE ServiceID = @ServiceID", con, transaction);
                    deleteServicePartsCmd.Parameters.AddWithValue("@ServiceID", serviceID);
                    deleteServicePartsCmd.ExecuteNonQuery();

                    // Delete from Services
                    SqlCommand deleteServicesCmd = new SqlCommand("DELETE FROM Services WHERE ServiceID = @ServiceID", con, transaction);
                    deleteServicesCmd.Parameters.AddWithValue("@ServiceID", serviceID);
                    deleteServicesCmd.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();

                    return Ok(new { Status = "200", Message = "Service deleted successfully" });
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    transaction.Rollback();
                    return StatusCode(500, new { Status = "100", Message = "Failed to delete service", Error = ex.Message });
                }
            }
        }
        [HttpGet]
        [Route("GetAllServices")]
        public IActionResult GetAllServices()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
            SELECT
                s.ServiceID,
                (SELECT c.Make FROM Cars c WHERE c.CarID = s.CarID) AS CarMake,
                (SELECT c.Model FROM Cars c WHERE c.CarID = s.CarID) AS CarModel,
                (SELECT cu.FirstName FROM Customers cu WHERE cu.CustomerID = (SELECT c.CustomerID FROM Cars c WHERE c.CarID = s.CarID)) AS CustomerFirstName,
                (SELECT cu.LastName FROM Customers cu WHERE cu.CustomerID = (SELECT c.CustomerID FROM Cars c WHERE c.CarID = s.CarID)) AS CustomerLastName,
                s.ServiceDate,
                s.Status,
                s.TotalCost,
                s.Description,
                s.MechanicId,
                s.ServiceTypeId,
                s.CarId
            FROM 
                Services s
            ORDER BY 
                s.ServiceDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                List<PendingService> pendingServicesList = new List<PendingService>();

                while (reader.Read())
                {
                    PendingService pendingService = new PendingService
                    {
                        ServiceID = Convert.ToInt32(reader["ServiceID"]),
                        CarMake = reader["CarMake"].ToString(),
                        CarModel = reader["CarModel"].ToString(),
                        CustomerFirstName = reader["CustomerFirstName"].ToString(),
                        CustomerLastName = reader["CustomerLastName"].ToString(),
                        ServiceDate = Convert.ToDateTime(reader["ServiceDate"]),
                        Status = reader["Status"].ToString(),
                        TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                        Description = reader["Description"].ToString(),
                        MechanicId = Convert.ToInt32(reader["MechanicId"]),
                        ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]),
                        CarId = Convert.ToInt32(reader["CarId"])
                    };

                    pendingServicesList.Add(pendingService);
                }

                if (pendingServicesList.Count > 0)
                {
                    return Ok(pendingServicesList);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "No pending services found" });
                }
            }
        }
        [HttpPost]
        [Route("AddCarPartToService/{serviceID}/{CarPartId}/{Quantity}")]
        public IActionResult AddCarPartToService(int serviceID, int CarPartId, int Quantity)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
            INSERT INTO ServiceParts (ServiceID, PartID, Quantity)
            VALUES (@ServiceID, @PartID, @Quantity)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ServiceID", serviceID);
                cmd.Parameters.AddWithValue("@PartID", CarPartId);
                cmd.Parameters.AddWithValue("@Quantity", Quantity);

                try
                {
                    cmd.ExecuteNonQuery();
                    return Ok(new { Status = "200", Message = "Car part added to service successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Status = "100", Message = "Failed to add car part to service", Error = ex.Message });
                }
            }
        }
        [HttpPut]
        [Route("UpdateService/{serviceID}")]
        public IActionResult UpdateService(int serviceID, [FromBody] Service service)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
        UPDATE Services
        SET 
            ServiceDate = @ServiceDate,
            Description = @Description,
            TotalCost = @TotalCost,
            Status = @Status,
            MechanicID = @MechanicID,
            ServiceTypeID = @ServiceTypeID
        WHERE 
            ServiceID = @ServiceID;
        ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ServiceDate", service.ServiceDate);
                cmd.Parameters.AddWithValue("@Description", service.Description);
                cmd.Parameters.AddWithValue("@TotalCost", service.TotalCost);
                cmd.Parameters.AddWithValue("@Status", service.Status);
                cmd.Parameters.AddWithValue("@MechanicID", service.MechanicId);
                cmd.Parameters.AddWithValue("@ServiceTypeID", service.ServiceTypeId);
                cmd.Parameters.AddWithValue("@ServiceID", serviceID);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Service updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Service not found" });
                }
            }
        }

        [HttpDelete]
        [Route("DeleteCarPartFromService/{serviceID}/{partID}")]
        public IActionResult DeleteCarPartFromService(int serviceID, int partID)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
            DELETE FROM ServiceParts
            WHERE ServiceID = @ServiceID AND PartID = @PartID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ServiceID", serviceID);
                cmd.Parameters.AddWithValue("@PartID", partID);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok(new { Status = "200", Message = "Car part deleted from service successfully" });
                    }
                    else
                    {
                        return NotFound(new { Status = "404", Message = "Car part not found in service" });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Status = "100", Message = "Failed to delete car part from service", Error = ex.Message });
                }
            }
        }

        [HttpGet]
        [Route("GetServicePartsAllByServiceId")]
        public IActionResult GetServicePartsAllByServiceId(int serviceId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
        SELECT 
            cp.PartID,
            cp.PartName,
            cp.Price,
            sp.Quantity
        FROM 
            ServiceParts sp
        JOIN 
            CarParts cp ON sp.PartID = cp.PartID
        WHERE 
            sp.ServiceID = @ServiceID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ServiceID", serviceId);

                SqlDataReader reader = cmd.ExecuteReader();
                List<ServicePart> serviceParts = new List<ServicePart>();

                while (reader.Read())
                {
                    serviceParts.Add(new ServicePart
                    {
                        PartId = Convert.ToInt32(reader["PartID"]),
                        PartName = reader["PartName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                return Ok(serviceParts);
            }
        }
        [HttpGet]
        [Route("GetAllCarParts")]
        public IActionResult GetAllCarParts()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
            SELECT PartId, PartName, Price AS PartPrice, Quantity AS PartQuantity
            FROM CarParts";

                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataReader reader = cmd.ExecuteReader();
                List<ServicePartDetail> carParts = new List<ServicePartDetail>();

                while (reader.Read())
                {
                    carParts.Add(new ServicePartDetail
                    {
                        CarPartId = Convert.ToInt32(reader["PartId"]),
                        PartName = reader["PartName"].ToString(),
                        PartPrice = Convert.ToDecimal(reader["PartPrice"]),
                        PartQuantity = Convert.ToInt32(reader["PartQuantity"])
                    });
                }

                return Ok(carParts);
            }
        }

        [HttpGet]
        [Route("GetAllMechanics")]
        public IActionResult GetAllMechanics()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
            SELECT 
                MechanicID, 
                FirstName, 
                LastName, 
                Email, 
                Phone 
            FROM 
                [ServiceAuto].[dbo].[Mechanics]";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                List<Mechanic> mechanics = new List<Mechanic>();

                while (reader.Read())
                {
                    mechanics.Add(new Mechanic
                    {
                        MechanicId = Convert.ToInt32(reader["MechanicID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString()
                    });
                }

                return Ok(mechanics);
            }
        }

        [HttpGet]
        [Route("GetServicesByMechanicId/{mechanicId}")]
        public IActionResult GetServicesByMechanicId(int mechanicId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
            SELECT
                s.ServiceID,
                (SELECT Make FROM Cars WHERE CarID = s.CarID) AS CarMake,
                (SELECT Model FROM Cars WHERE CarID = s.CarID) AS CarModel,
                (SELECT FirstName FROM Customers WHERE CustomerID = (SELECT CustomerID FROM Cars WHERE CarID = s.CarID)) AS CustomerFirstName,
                (SELECT LastName FROM Customers WHERE CustomerID = (SELECT CustomerID FROM Cars WHERE CarID = s.CarID)) AS CustomerLastName,
                s.ServiceDate,
                s.Status,
                s.TotalCost,
                s.Description,
                s.MechanicId,
                s.ServiceTypeId,
                s.CarId
            FROM 
                Services s
            WHERE 
                s.MechanicId = @MechanicId
            ORDER BY 
                s.ServiceDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MechanicId", mechanicId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<PendingService> pendingServicesList = new List<PendingService>();

                while (reader.Read())
                {
                    PendingService pendingService = new PendingService
                    {
                        ServiceID = Convert.ToInt32(reader["ServiceID"]),
                        CarMake = reader["CarMake"].ToString(),
                        CarModel = reader["CarModel"].ToString(),
                        CustomerFirstName = reader["CustomerFirstName"].ToString(),
                        CustomerLastName = reader["CustomerLastName"].ToString(),
                        ServiceDate = Convert.ToDateTime(reader["ServiceDate"]),
                        Status = reader["Status"].ToString(),
                        TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                        Description = reader["Description"].ToString(),
                        MechanicId = Convert.ToInt32(reader["MechanicId"]),
                        ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]),
                        CarId = Convert.ToInt32(reader["CarId"])
                    };

                    pendingServicesList.Add(pendingService);
                }

                if (pendingServicesList.Count > 0)
                {
                    return Ok(pendingServicesList);
                }
                else
                {
                    return NotFound(new { Status = "404", Message = "No services found for the specified mechanic" });
                }
            }
        }

        [HttpGet]
        [Route("statistics/total-services")]
        public IActionResult GetTotalServices()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Services";
                SqlCommand cmd = new SqlCommand(query, con);
                int totalServices = (int)cmd.ExecuteScalar();
                return Ok(totalServices);
            }
        }

        [HttpGet]
        [Route("statistics/total-revenue")]
        public IActionResult GetTotalRevenue()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();
                string query = "SELECT SUM(TotalCost) FROM Services";
                SqlCommand cmd = new SqlCommand(query, con);
                decimal totalRevenue = (decimal)cmd.ExecuteScalar();
                return Ok(totalRevenue);
            }
        }

        [HttpGet]
        [Route("statistics/average-service-cost")]
        public IActionResult GetAverageServiceCost()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();
                string query = "SELECT AVG(TotalCost) FROM Services";
                SqlCommand cmd = new SqlCommand(query, con);
                decimal averageServiceCost = (decimal)cmd.ExecuteScalar();
                return Ok(averageServiceCost);
            }
        }

        [HttpGet]
        [Route("statistics/top-customers")]
        public IActionResult GetTopCustomers()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();
                string query = @"
            SELECT TOP 5 c.CustomerID, c.FirstName, c.LastName, COUNT(s.ServiceID) AS ServiceCount
            FROM Customers c
            JOIN Cars ca ON c.CustomerID = ca.CustomerID
            JOIN Services s ON ca.CarID = s.CarID
            GROUP BY c.CustomerID, c.FirstName, c.LastName
            ORDER BY ServiceCount DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                List<object> topCustomers = new List<object>();
                while (reader.Read())
                {
                    topCustomers.Add(new
                    {
                        CustomerID = reader["CustomerID"],
                        FirstName = reader["FirstName"],
                        LastName = reader["LastName"],
                        ServiceCount = reader["ServiceCount"]
                    });
                }
                return Ok(topCustomers);
            }
        }

        [HttpGet]
        [Route("statistics/mechanic-performance")]
        public IActionResult GetMechanicPerformance()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();
                string query = @"
            SELECT m.MechanicID, m.FirstName, m.LastName, COUNT(s.ServiceID) AS ServiceCount
            FROM Mechanics m
            JOIN Services s ON m.MechanicID = s.MechanicId
            GROUP BY m.MechanicID, m.FirstName, m.LastName
            ORDER BY ServiceCount DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                List<object> mechanicPerformance = new List<object>();
                while (reader.Read())
                {
                    mechanicPerformance.Add(new
                    {
                        MechanicID = reader["MechanicID"],
                        FirstName = reader["FirstName"],
                        LastName = reader["LastName"],
                        ServiceCount = reader["ServiceCount"]
                    });
                }
                return Ok(mechanicPerformance);
            }
        }

        [HttpGet]
        [Route("statistics/low-stock-parts")]
        public IActionResult GetLowStockParts()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();
                string query = "SELECT TOP 5 PartID, PartName, Quantity FROM CarParts WHERE Quantity < 10 ORDER BY Quantity ASC";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                List<object> lowStockParts = new List<object>();
                while (reader.Read())
                {
                    lowStockParts.Add(new
                    {
                        PartID = reader["PartID"],
                        PartName = reader["PartName"],
                        StockQuantity = reader["Quantity"]
                    });
                }
                return Ok(lowStockParts);
            }
        }

        [HttpGet]
        [Route("statistics/service-type-analysis")]
        public IActionResult GetServiceTypeAnalysis()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();
                string query = @"
            SELECT st.ServiceTypeID, st.ServiceName, COUNT(s.ServiceID) AS ServiceCount
            FROM ServiceTypes st
            JOIN Services s ON st.ServiceTypeID = s.ServiceTypeId
            GROUP BY st.ServiceTypeID, st.ServiceName
            ORDER BY ServiceCount DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                List<object> serviceTypeAnalysis = new List<object>();
                while (reader.Read())
                {
                    serviceTypeAnalysis.Add(new
                    {
                        ServiceTypeID = reader["ServiceTypeID"],
                        ServiceTypeName = reader["ServiceName"],
                        ServiceCount = reader["ServiceCount"]
                    });
                }
                return Ok(serviceTypeAnalysis);
            }
        }

        [HttpPut]
        [Route("UpdateCar")]
        public IActionResult UpdateCar([FromBody] Car car)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                // Check if the car exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM [ServiceAuto].[dbo].[Cars] WHERE CarID = @CarID", con);
                checkCmd.Parameters.AddWithValue("@CarID", car.CarID);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    return NotFound(new { Status = "404", Message = "Car not found" });
                }

                // Update the car details
                SqlCommand cmd = new SqlCommand("UPDATE [ServiceAuto].[dbo].[Cars] SET Make = @Make, Model = @Model, Year = @Year, VIN = @VIN, LicensePlate = @LicensePlate WHERE CarID = @CarID", con);
                cmd.Parameters.AddWithValue("@Make", car.Make);
                cmd.Parameters.AddWithValue("@Model", car.Model);
                cmd.Parameters.AddWithValue("@Year", car.Year);
                cmd.Parameters.AddWithValue("@VIN", car.VIN);
                cmd.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
                cmd.Parameters.AddWithValue("@CarID", car.CarID);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { Status = "200", Message = "Car updated successfully" });
                }
                else
                {
                    return StatusCode(500, new { Status = "100", Message = "Failed to update car" });
                }
            }
        }

        [HttpGet("service-type-distribution")]
        public IActionResult GetServiceTypeDistribution()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
                SELECT 
                    st.ServiceName AS ServiceTypeName,
                    COUNT(s.ServiceID) AS ServiceCount,
                    SUM(s.TotalCost) AS TotalRevenue
                FROM 
                    Services s
                JOIN 
                    ServiceTypes st ON s.ServiceTypeID = st.ServiceTypeID
                GROUP BY 
                    st.ServiceName
                ORDER BY 
                    ServiceCount DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                List<ServiceTypeDistribution> serviceTypeDistributions = new List<ServiceTypeDistribution>();

                while (reader.Read())
                {
                    ServiceTypeDistribution distribution = new ServiceTypeDistribution
                    {
                        ServiceTypeName = reader["ServiceTypeName"].ToString(),
                        ServiceCount = Convert.ToInt32(reader["ServiceCount"]),
                        TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
                    };

                    serviceTypeDistributions.Add(distribution);
                }

                return Ok(serviceTypeDistributions);
            }
        }
        [HttpGet("top-mechanics-by-revenue")]
        public IActionResult GetTopMechanicsByRevenue()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
                SELECT 
                    m.MechanicID,
                    m.FirstName,
                    m.LastName,
                    COUNT(s.ServiceID) AS ServiceCount,
                    SUM(s.TotalCost) AS TotalRevenue
                FROM 
                    Mechanics m
                JOIN 
                    Services s ON m.MechanicID = s.MechanicID
                GROUP BY 
                    m.MechanicID, m.FirstName, m.LastName
                ORDER BY 
                    TotalRevenue DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                List<MechanicRevenue> mechanicRevenues = new List<MechanicRevenue>();

                while (reader.Read())
                {
                    MechanicRevenue revenue = new MechanicRevenue
                    {
                        MechanicID = Convert.ToInt32(reader["MechanicID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        ServiceCount = Convert.ToInt32(reader["ServiceCount"]),
                        TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
                    };

                    mechanicRevenues.Add(revenue);
                }

                return Ok(mechanicRevenues);
            }
        }

        [HttpGet("monthly-revenue-and-service-count")]
        public IActionResult GetMonthlyRevenueAndServiceCount()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                con.Open();

                string query = @"
                SELECT 
                    YEAR(s.ServiceDate) AS Year,
                    MONTH(s.ServiceDate) AS Month,
                    COUNT(s.ServiceID) AS ServiceCount,
                    SUM(s.TotalCost) AS TotalRevenue
                FROM 
                    Services s
                GROUP BY 
                    YEAR(s.ServiceDate), MONTH(s.ServiceDate)
                ORDER BY 
                    Year, Month";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                List<MonthlyRevenue> monthlyRevenues = new List<MonthlyRevenue>();

                while (reader.Read())
                {
                    MonthlyRevenue revenue = new MonthlyRevenue
                    {
                        Year = Convert.ToInt32(reader["Year"]),
                        Month = Convert.ToInt32(reader["Month"]),
                        ServiceCount = Convert.ToInt32(reader["ServiceCount"]),
                        TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
                    };

                    monthlyRevenues.Add(revenue);
                }

                return Ok(monthlyRevenues);
            }
        }


    }
}

