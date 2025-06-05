const apiBaseUrl = 'http://localhost:5132/api/school';

function toggleMenu() {
    const menu = document.getElementById('menu');
    menu.classList.toggle('hidden');
}

function showSection(sectionId) {
    document.querySelectorAll('.section').forEach(section => section.classList.add('hidden'));
    document.getElementById(sectionId).classList.remove('hidden');
    toggleMenu();
}

async function addStudent(event) {
    event.preventDefault();
    const name = document.getElementById('student-name').value;
    const gender = document.getElementById('student-gender').value;
    const dob = document.getElementById('student-dob').value;
    const className = document.getElementById('student-class').value;

    try {
        const response = await fetch(`${apiBaseUrl}/students`, { // Changed API_BASE_URL to apiBaseUrl
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, gender, dateOfBirth: dob, className })
        });
        const result = await response.json();
        if (response.ok) {
            alert(result.message);
            document.getElementById('add-student-form').reset();
        } else {
            alert(result.message);
        }
    } catch (error) {
        alert('Error adding student: ' + error.message);
    }
}

async function deleteStudent(event) {
    event.preventDefault();
    const id = document.getElementById('student-id').value;

    try {
        const response = await fetch(`${apiBaseUrl}/students/${id}`, {
            method: 'DELETE'
        });
        const result = await response.json();
        if (response.ok) {
            alert(result.message);
            document.getElementById('delete-student-form').reset();
        } else {
            alert(result.message);
        }
    } catch (error) {
        alert('Error deleting student: ' + error.message);
    }
}

async function getAllStudents() {
    try {
        const response = await fetch(`${apiBaseUrl}/students`);
        const students = await response.json();
        const list = document.getElementById('students-list');
        list.innerHTML = students.length ?
            '<ul>' + students.map(s => `<li>ID: ${s.id}, Name: ${s.name}, Gender: ${s.gender}, DOB: ${new Date(s.dateOfBirth).toLocaleDateString()}, Class: ${s.className}</li>`).join('') + '</ul>' :
            'No students found.';
    } catch (error) {
        alert('Error fetching students: ' + error.message);
    }
}

async function getStudentByName(event) {
    event.preventDefault();
    const name = document.getElementById('search-student-name').value;

    try {
        const response = await fetch(`${apiBaseUrl}/students/name/${encodeURIComponent(name)}`);
        const result = await response.json();
        const display = document.getElementById('student-search-result');
        if (response.ok) {
            display.innerHTML = `ID: ${result.id}, Name: ${result.name}, Gender: ${result.gender}, DOB: ${new Date(result.dateOfBirth).toLocaleDateString()}, Class: ${result.className}`;
        } else {
            display.innerHTML = result.message;
        }
    } catch (error) {
        alert('Error searching student: ' + error.message);
    }
}

async function getStudentsByClass(event) {
    event.preventDefault();
    const className = document.getElementById('search-class-name').value;

    try {
        const response = await fetch(`${apiBaseUrl}/students/class/${encodeURIComponent(className)}`);
        const students = await response.json();
        const display = document.getElementById('students-class-result');
        display.innerHTML = students.length ?
            '<ul>' + students.map(s => `<li>ID: ${s.id}, Name: ${s.name}, Gender: ${s.gender}, DOB: ${new Date(s.dateOfBirth).toLocaleDateString()}</li>`).join('') + '</ul>' :
            'No students found in this class.';
    } catch (error) {
        alert('Error searching students by class: ' + error.message);
    }
}

async function addClassroom(event) {
    event.preventDefault();
    const roomName = document.getElementById('classroom-name').value;
    const size = parseFloat(document.getElementById('classroom-size').value);
    const capacity = parseInt(document.getElementById('classroom-capacity').value);
    const hasCynapSystem = document.getElementById('classroom-cynap').checked;

    try {
        const response = await fetch(`${apiBaseUrl}/classrooms`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ roomName, size, capacity, hasCynapSystem })
        });
        const result = await response.json();
        if (response.ok) {
            alert(result.message);
            document.getElementById('add-classroom-form').reset();
        } else {
            alert(result.message);
        }
    } catch (error) {
        alert('Error adding classroom: ' + error.message);
    }
}

async function deleteClassroom(event) {
    event.preventDefault();
    const id = document.getElementById('classroom-id').value;

    try {
        const response = await fetch(`${apiBaseUrl}/classrooms/${id}`, {
            method: 'DELETE'
        });
        const result = await response.json();
        if (response.ok) {
            alert(result.message);
            document.getElementById('delete-classroom-form').reset();
        } else {
            alert(result.message);
        }
    } catch (error) {
        alert('Error deleting classroom: ' + error.message);
    }
}

async function getAllClassrooms() {
    try {
        const response = await fetch(`${apiBaseUrl}/classrooms`);
        const classrooms = await response.json();
        const list = document.getElementById('classrooms-list');
        list.innerHTML = classrooms.length ?
            '<ul>' + classrooms.map(c => `<li>ID: ${c.id}, Name: ${c.roomName}, Size: ${c.size} sqm, Capacity: ${c.capacity}, Cynap: ${c.hasCynapSystem}</li>`).join('') + '</ul>' :
            'No classrooms found.';
    } catch (error) {
        alert('Error fetching classrooms: ' + error.message);
    }
}

async function getClassroomByName(event) {
    event.preventDefault();
    const roomName = document.getElementById('search-classroom-name').value;

    try {
        const response = await fetch(`${apiBaseUrl}/classrooms/${encodeURIComponent(roomName)}`);
        const result = await response.json();
        const display = document.getElementById('classroom-search-result');
        if (response.ok) {
            display.innerHTML = `ID: ${result.id}, Name: ${result.roomName}, Size: ${result.size} sqm, Capacity: ${result.capacity}, Cynap: ${result.hasCynapSystem}`;
        } else {
            display.innerHTML = result.message;
        }
    } catch (error) {
        alert('Error searching classroom: ' + error.message);
    }
}

async function checkClassFit(event) {
    event.preventDefault();
    const className = document.getElementById('fit-class-name').value;
    const roomName = document.getElementById('fit-room-name').value;

    try {
        const response = await fetch(`${apiBaseUrl}/classroom/fit?className=${encodeURIComponent(className)}&roomName=${encodeURIComponent(roomName)}`);
        const result = await response.json();
        const display = document.getElementById('fit-result');
        if (response.ok) {
            display.innerHTML = `Class ${result.className} ${result.canFit ? 'can' : 'cannot'} fit in room ${result.roomName}.`;
        } else {
            display.innerHTML = result.message;
        }
    } catch (error) {
        alert('Error checking fit: ' + error.message);
    }
}