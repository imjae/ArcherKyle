// ��� �������̽�
// : ������ ����, Ȱ�뿡 ���� Ÿ�� ����

public interface ISubject
{
    void AddObserver(Observer o);
    void RemoveObserver(Observer o);
    void Notify();
}
