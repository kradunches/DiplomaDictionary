import http from 'k6/http';
import {check, sleep} from 'k6';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

// слабая нагрузка
export const options = {
    vus: 3,
    duration: '5m',
    thresholds: {
        http_req_failed: ['rate<0.01'], // не более 1% ошибок
        http_req_duration: [
            'p(95)<500',// 95% запросов быстрее 500мс
            'p(99)<1200',
            'avg<500'
        ]
    }
};

export default () => {
    const urlRes = http.get('http://localhost:5093/controller/GetAllTerms');
    check(urlRes, {'status returned 200': (r) => r.status == 200})
    sleep(1); //seconds
}

export function handleSummary(data) {
    return {
        'results/smoke_getallterms_report.html': htmlReport(data),
    };
}